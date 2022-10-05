namespace CatConsult.PaginationHelper
{
    public interface IPaginateResult<T>
    {
        IEnumerable<T> Data { get; set; }
        int Count { get; set; }
    }
}
