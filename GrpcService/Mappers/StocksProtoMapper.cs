using Riok.Mapperly.Abstractions;
using GrpcService.Entities;
using Stocks;

namespace GrpcService.Mappers;

[Mapper]
public partial class StocksProtoMapper
{
    public partial Stock MapToProto(Entities.Stocks source);

    public partial IEnumerable<Stock> MapToProtoList(
        IEnumerable<Entities.Stocks> source);

    public StocksResponse MapToResponse(
        IEnumerable<Entities.Stocks> entities,
        int totalCount)
    {
        var response = new StocksResponse
        {
            TotalCount = totalCount
        };

        response.Stocks.AddRange(MapToProtoList(entities));

        return response;
    }
}