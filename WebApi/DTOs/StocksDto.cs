namespace WebApi.DTOs
{
    public class StocksDto
    {
        public string? ProfileId { get; set; }
        public string? CarName { get; set; }
        public string? FormattedPrice { get; set; }
        public string? EmiText { get; set; }
        public string? Fuel { get; set; }
        public int KmsDriven { get; set; }
        public string? CityName { get; set; }
        public bool IsValueForMoney { get; set; }

        public List<string> ImageUrls { get; set; } = [];
    }
}