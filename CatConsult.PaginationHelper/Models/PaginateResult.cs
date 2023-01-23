namespace CatConsult.PaginationHelper
{
    public class PaginateResult<T> : IPaginateResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();

        public int Count { get; set; }

        public int CurrentPage { get; set; }

        public int RowsPerPage { get; set; }

        public int TotalPages => RowsPerPage > 0 ? (int)Math.Ceiling(decimal.Divide(Count, RowsPerPage)) : Count;

        public int? PreviousPage => CurrentPage > 0 ? CurrentPage - 1 : null;

        public int? NextPage => CurrentPage < (TotalPages - 1) ? CurrentPage + 1 : null;
    }
}
