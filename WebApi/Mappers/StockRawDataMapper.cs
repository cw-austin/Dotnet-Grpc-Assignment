using Riok.Mapperly.Abstractions;
using WebApi.Entities.StocksRawData;
using Stocks; // gRPC generated namespace
using Google.Protobuf.Collections;

namespace WebApi.Mappers;

[Mapper]
public partial class StockRawDataMapper
{
    [MapProperty(nameof(Stock.ImageUrls), nameof(StockRawData.ImageUrls), Use = nameof(MapUrls))]
    public partial StockRawData Map(Stock source);

    public partial List<StockRawData> Map(List<Stock> source);

    private static List<string> MapUrls(RepeatedField<string>? urls) => 
        urls?.ToList() ?? new List<string>();
}
