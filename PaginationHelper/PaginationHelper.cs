using System.Collections;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace PaginationHelper
{
    /// <summary>
    /// This class contains IQueryable extension method to apply pagination logic
    /// </summary>
    public static class PaginationHelper
    {

        /// <summary>
        /// Dynamically filter, sort, and paginate a query
        /// </summary>
        /// <typeparam name="T">Query Item Type</typeparam>
        /// <param name="query">Queryable list</param>
        /// <param name="optionsBuilder">options builder to create options to filter</param>
        /// <param name="transform">Optional transform the query after apply pagination options</param>
        /// <returns></returns>
        public static Task<IPaginateResult<T>> ToPaginatedAsync<T>
        (
            this IQueryable<T> query,
            IPaginateOptionsBuilder optionsBuilder,
            Func<IQueryable<T>, IQueryable<T>> transform = null)
        {
            return query.ToPaginatedAsync(optionsBuilder?.Build(), transform);
        }

        /// <summary>
        /// Dynamically filter, sort, and paginate a query
        /// </summary>
        /// <typeparam name="T">Query Item Type</typeparam>
        /// <param name="query">Queryable list</param>
        /// <param name="options">Options to perform filter, search, and sort</param>
        /// <param name="transform">Optional transform the query after apply pagination options</param>
        /// <returns></returns>
        public static async Task<IPaginateResult<T>> ToPaginatedAsync<T>
        (
            this IQueryable<T> query,
            PaginateOptions options,
            Func<IQueryable<T>, IQueryable<T>> transform = null)
        {

            options ??= new PaginateOptions();

            var shouldSearch = !string.IsNullOrEmpty(options.Search) && options.Columns.Any();

            Dictionary<string, FilterPropertyType> propertyTypes = null;

            // use reflection to get property types for filter and search
            if (options.Filters.Any() || shouldSearch)
                propertyTypes = typeof(T).GetProperties()
                   .ToDictionary
                   (
                       p => p.Name.ToLower(),
                       p => MapPropertyType(p.PropertyType)
                   );

            foreach (var filterEntry in options.Filters)
            {
                if (propertyTypes.TryGetValue(filterEntry.Key, out var filterPropertyType))
                {
                    // filter by each individual columns
                    var filterQueryStr = DynamicTranslate(filterPropertyType, filterEntry.Key, filterEntry.Value);
                    // filter query use AND
                    query = query.Where(filterQueryStr);
                }
            }

            if (shouldSearch)
            {
                // search all columns by search keyword
                var searchQueryStr = options.Columns
                    .Select(c => c.ToLower())
                    .Where(c => propertyTypes.ContainsKey(c))
                    .Select(c => DynamicTranslate(propertyTypes[c], c, options.Search));

                // search query use OR
                query = query.Where(string.Join(" || ", searchQueryStr));
            }

            if (!string.IsNullOrEmpty(options.OrderBy))
            {
                // sort query
                query = query.OrderBy($"{options.OrderBy} {options.OrderDirection}");
            }

            if (transform != null)
            {
                query = transform(query);
            }

            var count = await query.CountAsync();

            // only skip and take if RowsPerPage is greater than 0
            if (options.RowsPerPage > 0)
            {
                query = query
                    .Skip(options.Page * options.RowsPerPage)
                    .Take(options.RowsPerPage);
            }

            var result = await query.ToListAsync();

            return new PaginateResult<T>()
            {
                Count = count,
                Data = result
            };
        }

        private static FilterPropertyType MapPropertyType(Type t)
        {
            var underlyingType = Nullable.GetUnderlyingType(t);
            if (underlyingType != null)
            {
                t = underlyingType;
            }
            if (t.Equals(typeof(string)))
            {
                return FilterPropertyType.String;
            }
            else if (typeof(DateTime).IsAssignableFrom(t) || typeof(DateTimeOffset).IsAssignableFrom(t) || typeof(TimeSpan).IsAssignableFrom(t))
            {
                return FilterPropertyType.DateTime;
            }
            else if (Type.GetTypeCode(t) is TypeCode.Byte or TypeCode.SByte or TypeCode.UInt16 or
                TypeCode.UInt32 or TypeCode.UInt64 or TypeCode.Int16 or TypeCode.Int32 or
                TypeCode.Int64 or TypeCode.Decimal or TypeCode.Double or TypeCode.Single)
            {
                return FilterPropertyType.Number;
            }
            else if (typeof(IEnumerable).IsAssignableFrom(t))
            {
                return FilterPropertyType.List;
            }
            return FilterPropertyType.Other;
        }

        private static string DynamicTranslate(FilterPropertyType ptype, string name, IEnumerable<PaginateFilterValue> values)
        {
            var result = new List<string>();

            // use AND for range filter. For example: 5 < quantity and quantity < 10
            var rangeFilterValues = string.Join(" && ", values
                .Where(v => v.IsRangeFilterType)
                .Select(v => DynamicTranslate(ptype, name, v)));

            if (!string.IsNullOrWhiteSpace(rangeFilterValues))
            {
                result.Add($"({rangeFilterValues})");
            }

            // use OR for non range filter. For example: name = "A" or name = "B"
            var nonRangeFilterValues = string.Join(" || ", values
                .Where(v => !v.IsRangeFilterType)
                .Select(v => DynamicTranslate(ptype, name, v)));

            if (!string.IsNullOrWhiteSpace(nonRangeFilterValues))
            {
                result.Add($"({nonRangeFilterValues})");
            }

            if (result.Count > 1)
            {
                return string.Join(" || ", result);
            }

            return result.First();
        }

        private static string DynamicTranslate(FilterPropertyType ptype, string name, PaginateFilterValue pval)
        {
            return DynamicTranslate(ptype, name, pval.Value, pval.FilterType);
        }
        private static string DynamicTranslate(FilterPropertyType ptype, string name, string value, PaginateFilterType? filterType = null)
        {
            if (!filterType.HasValue)
            {
                filterType = ptype is FilterPropertyType.String or FilterPropertyType.List
                    ? PaginateFilterType.In
                    : PaginateFilterType.Equal;
            };

            switch (filterType)
            {
                case PaginateFilterType.LessThan:
                    return $"{name} < \"{value}\"";
                case PaginateFilterType.LessThanOrEqual:
                    return $"{name} <= \"{value}\"";
                case PaginateFilterType.GreaterThan:
                    return $"{name} > \"{value}\"";
                case PaginateFilterType.GreaterThanOrEqual:
                    return $"{name} >= \"{value}\"";
                case PaginateFilterType.In:
                    if (ptype == FilterPropertyType.String)
                    {
                        return $"{name}.ToLower().Contains(\"{value.ToLower()}\")";
                    }
                    return $"{name}.Contains(\"{value}\")";
                case PaginateFilterType.Equal:
                    if (ptype == FilterPropertyType.String)
                    {
                        return $"{name}.ToLower() == \"{value.ToLower()}\"";
                    }
                    else if (ptype == FilterPropertyType.Number)
                    {
                        return double.TryParse(value, out var num)
                            ? $"{name} == \"{num}\""
                            : "false";
                    }
                    else if (ptype == FilterPropertyType.DateTime)
                    {
                        // compare to dates..
                        if (DateTime.TryParse(value, out var datetime))
                        {
                            var start = new DateTimeOffset(new DateTime(datetime.Year, datetime.Month, datetime.Day).ToUniversalTime());
                            var end = start.AddDays(1).AddMilliseconds(-1);
                            return $"(\"{start}\" <= {name} && {name} <= \"{end}\")";
                        }
                        return "false";
                    }
                    else
                    {
                        return $"{name} == \"{value}\"";
                    }
                case PaginateFilterType.StartWith:
                    return $"{name}.ToLower().StartsWith(\"{value.ToLower()}\")";
                case PaginateFilterType.EndWith:
                    return $"{name}.ToLower().EndsWith(\"{value.ToLower()}\")";
            }
            return string.Empty;
        }
    }
}
