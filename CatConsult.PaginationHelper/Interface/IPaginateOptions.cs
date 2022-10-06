namespace CatConsult.PaginationHelper
{
    public interface IPaginateOptions
    {
        string OrderBy { get; set; }
        string OrderDirection { get; set; }
        IDictionary<string, List<PaginateFilterValue>> Filters { get; set; }
        ISet<string> Columns { get; set; }
        int RowsPerPage { get; set; }
        int Page { get; set; }
        string Search { get; set; }
    }
}
