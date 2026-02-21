using Riok.Mapperly.Abstractions;
using WebApi.Domain.Entities.StocksRawData;
using WebApi.Domain.DTOs;

namespace WebApi.Mappers;

[Mapper]
public partial class StocksDtoMapper
{
    // 1. Ignore source fields that are combined into CarName
    [MapperIgnoreSource(nameof(StockRawData.MakeYear))]
    [MapperIgnoreSource(nameof(StockRawData.MakeName))]
    [MapperIgnoreSource(nameof(StockRawData.ModelName))]
    // 2. Map target fields using custom methods
    [MapProperty(new[] { nameof(StockRawData.Price) }, new[] { nameof(StocksDto.FormattedPrice) })]
    [MapProperty(nameof(StockRawData), nameof(StocksDto.CarName))]
    [MapProperty(nameof(StockRawData), nameof(StocksDto.IsValueForMoney))]
    public partial StocksDto Map(StockRawData source);

    public partial List<StocksDto> Map(List<StockRawData> source);

    // Custom Mapping Logic
    private string MapFormattedPrice(int price) =>
        price >= 10_000_000
            ? $"Rs. {price / 10_000_000.0:0.#} Crore"
            : price >= 100_000
                ? $"Rs. {price / 100_000.0:0.#} Lakh"
                : $"Rs. {price:N0}";

    private string MapCarName(StockRawData stock) =>
        $"{stock.MakeYear} {stock.MakeName} {stock.ModelName}".Trim();

    private bool MapIsValueForMoney(StockRawData stock) =>
        stock.KmsDriven < 10000 && stock.Price < 200_000;
}