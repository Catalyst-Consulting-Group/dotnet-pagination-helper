using System.Text.RegularExpressions;

namespace CatConsult.PaginationHelper
{
    public class PaginateOptions: IPaginateOptions
    {
        public PaginateOptions()
        {
        }

        public PaginateOptions(IDictionary<string, ICollection<string>> queryParams, ISet<string> excludingSet = null, ISet<string> includingSet = null)
        {
            var noIncludingSet = includingSet == null || !includingSet.Any();
            if (queryParams != null)
            {
                foreach (var item in queryParams)
                {
                    if (item.Value.Any())
                    {
                        // to lower case and filter out []
                        // example: HelloWorld[0] -> helloworld
                        var loweredKey = Regex.Replace(item.Key.ToLower(), @"\[.*\]", "");
                        var filterType = Regex.Match(loweredKey, @"(__\w*)$");

                        if (filterType.Success)
                            loweredKey = loweredKey.Replace(filterType.Value, "");

                        if (loweredKey == nameof(Search).ToLower())
                        {
                            Search = item.Value.First();
                        }
                        else if (loweredKey == nameof(OrderBy).ToLower())
                        {
                            OrderBy = item.Value.First();
                        }
                        else if (loweredKey == nameof(OrderDirection).ToLower())
                        {
                            OrderDirection = item.Value.First();
                        }
                        else if (loweredKey == nameof(Page).ToLower())
                        {
                            Page = int.Parse(item.Value.First());
                        }
                        else if (loweredKey == nameof(RowsPerPage).ToLower())
                        {
                            RowsPerPage = int.Parse(item.Value.First());
                        }
                        else if (loweredKey == nameof(Columns).ToLower())
                        {
                            if (noIncludingSet)
                            {
                                // incase ["a,b", "c"], join then split will yield ["a", "b", "c"]
                                var strVal = string.Join(",", item.Value).Split(",");

                                foreach(var col in strVal)
                                {
                                    var loweredCol = col.ToLower();
                                    if (excludingSet == null || !excludingSet.Contains(loweredCol))
                                    {
                                        Columns.Add(loweredCol);
                                    }
                                }
                            }
                        }
                        else if (
                              (excludingSet == null || !excludingSet.Contains(loweredKey)) &&
                              (noIncludingSet || includingSet.Contains(loweredKey)))
                        {
                            if (!Filters.TryGetValue(loweredKey, out var values))
                            {
                                values = Filters[loweredKey] = new List<PaginateFilterValue>();
                            }

                            values.AddRange(item.Value.Select(v => new PaginateFilterValue(v, filterType.Value)));

                        }


                        // use including set as the column if provided
                        if (includingSet.Any())
                        {
                            Columns = includingSet;
                        }
                    }
                }
            }
        }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; } = "ASC";
        public IDictionary<string, List<PaginateFilterValue>> Filters { get; set; } = new Dictionary<string, List<PaginateFilterValue>>();
        public ISet<string> Columns { get; set; } = new HashSet<string>();
        public int RowsPerPage { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }

    }
}
