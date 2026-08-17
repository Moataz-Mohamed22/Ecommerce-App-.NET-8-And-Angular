public class ProductParams
{
    public string? Sort { get; set; }

    public int? CategoryId { get; set; }

    public int MaxPageSize { get; set; } = 6;
    public string ?Search { get; set; }
    private int _pageSize;

    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
    public int PageNumber { get; set; }
}