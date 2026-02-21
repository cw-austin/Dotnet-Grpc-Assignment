using Contracts.Entities;
using Grpc.Core;
using Stocks;
using WebApi.Domain.Entities.StocksRawData;
using WebApi.Mappers;

namespace WebApi.Repository.Repositories;

public class StocksRepository : IStocksRepository
{
    private readonly StockService.StockServiceClient _client;
    private readonly FiltersRequestMapper _filtersRequestMapper;
    private readonly StockRawDataMapper _stockMapper;

    public StocksRepository(
        StockService.StockServiceClient client,
        FiltersRequestMapper filtersRequestMapper,
        StockRawDataMapper stockMapper)
    {
        _client = client;
        _filtersRequestMapper = filtersRequestMapper;
        _stockMapper = stockMapper;
    }

    public async Task<(List<StockRawData> Stocks, int TotalCount)> GetStocksAsync(Filters filters)
    {
        try
        {
            var request = _filtersRequestMapper.ToGrpc(filters);
            var response = await _client.GetStocksAsync(request);

            var stocks = _stockMapper.Map(response.Stocks.ToList());
            return (stocks, response.TotalCount); 
        }
        catch (RpcException rpcEx)
        {
            Console.WriteLine($"gRPC call failed: {rpcEx.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StocksRepository.GetStocksAsync error: {ex}");
            throw;
        }
    }
}
