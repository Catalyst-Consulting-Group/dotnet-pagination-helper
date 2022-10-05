namespace CatConsult.PaginationHelper
{
    public class PaginateResult<T> : IPaginateResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int Count { get; set; }

    }
}
