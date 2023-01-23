namespace CatConsult.PaginationHelper
{
    public interface IPaginateResult<T>
    {
        IEnumerable<T> Data { get; set; }
        int Count { get; set; }

        int CurrentPage { get; set; }

        int RowsPerPage { get; set; }

        int TotalPages { get; }

        int? PreviousPage { get; }

        int? NextPage { get; }
    }
}
