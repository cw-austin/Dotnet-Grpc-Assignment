using WebApi.Entities.StocksRawData;
using WebApi.DTOs;
using Stocks;
using WebApi.Mappers;
using Grpc.Core;
using Contracts.Entities;

namespace WebApi.Repositories;

public class StocksRepository : IStocksRepository
{
    private readonly StockService.StockServiceClient _client;
    private readonly FiltersRequestMapper _filtersMapper;
    private readonly StockRawDataMapper _stockMapper;

    public StocksRepository(
        StockService.StockServiceClient client,
        FiltersRequestMapper filtersMapper,
        StockRawDataMapper stockMapper)
    {
        _client = client;
        _filtersMapper = filtersMapper;
        _stockMapper = stockMapper;
    }

    public async Task<(List<StockRawData> Stocks, int TotalCount)> GetStocksAsync(Filters filters)
    {
        try
        {
            var request = _filtersMapper.ToGrpc(filters);
            var response = await _client.GetStocksAsync(request);

            // Map the stocks list
            var stocks = _stockMapper.Map(response.Stocks.ToList());

            // Return both the list and the TotalCount from the gRPC response
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
    public async Task<List<CityItem>> GetCitiesAsync()
    {
        // google.protobuf.Empty is passed as 'new()' or 'new Empty()'
        var response = await _client.GetCitiesAsync(new()); 
        return response.Cities.ToList();
    }

    public async Task<List<MakeItem>> GetMakesAsync()
    {
        // google.protobuf.Empty is passed as 'new()' or 'new Empty()'
        var response = await _client.GetMakesAsync(new());
        return response.Makes.ToList();
    }
}
