namespace GrpcService.Entities.StockRow;

public class StockRow
{
    public string? ProfileId { get; set; }
    public string? MakeName { get; set; }
    public string? ModelName { get; set; }
    public int MakeYear { get; set; }
    public int Price { get; set; }
    public string? CityName { get; set; }
    public string? Fuel { get; set; }
    public int? KmsDriven { get; set; }
    public string? EmiText { get; set; }
    public string? ImageUrl { get; set; }
}
