public class PagedResponse<T>
{
    public IEnumerable<T> Stocks { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
}