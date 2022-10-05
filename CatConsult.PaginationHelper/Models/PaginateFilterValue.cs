namespace CatConsult.PaginationHelper
{
    public class PaginateFilterValue
    {

        public PaginateFilterValue()
        {
        }

        public PaginateFilterValue(string value, string filterType = null)
        {
            Value = value;

            switch (filterType)
            {
                case "__eq":
                    FilterType = PaginateFilterType.Equal;
                    break;
                case "__in":
                    FilterType = PaginateFilterType.In;
                    break;
                case "__start":
                    FilterType = PaginateFilterType.StartWith;
                    break;
                case "__end":
                    FilterType = PaginateFilterType.EndWith;
                    break;
                case "__gt":
                    FilterType = PaginateFilterType.GreaterThan;
                    break;
                case "__gte":
                    FilterType = PaginateFilterType.GreaterThanOrEqual;
                    break;
                case "__lt":
                    FilterType = PaginateFilterType.LessThan;
                    break;
                case "__lte":
                    FilterType = PaginateFilterType.LessThanOrEqual;
                    break;
                default:
                    // string or list type default in PaginateFilterType.In
                    // range type (number, datetime etc) default in PaginateFilterType.Equal
                    break;
            }
        }

        public PaginateFilterType? FilterType { get; set; }
        public string Value { get; set; }

        public bool IsRangeFilterType => FilterType is PaginateFilterType.GreaterThan or PaginateFilterType.GreaterThanOrEqual or PaginateFilterType.LessThan or PaginateFilterType.LessThanOrEqual;
    }
}
