namespace WebApi.Entities.StocksRawData;

public class StockRawData
{
    public string? ProfileId { get; set; }
    public int Price { get; set; }
    public int MakeYear { get; set; }
    public string? MakeName { get; set; }
    public string? ModelName { get; set; }
    public string? EmiText { get; set; }
    public string? Fuel { get; set; }
    public int KmsDriven { get; set; }
    public string? CityName { get; set; }

    public List<string> ImageUrls { get; set; } = new();
}
