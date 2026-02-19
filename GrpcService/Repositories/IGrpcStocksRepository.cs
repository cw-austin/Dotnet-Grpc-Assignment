using System;
namespace GrpcService.Repositories;
using EntityStocks = GrpcService.Entities.Stocks;
using Contracts.Entities;
using GrpcService.Entities.CityItem;
using GrpcService.Entities.MakeItem;

public interface IGrpcStocksRepository
{
    public Task<(List<EntityStocks> Stocks, int TotalCount)> GetStocksRepository(Filters filters);
    public Task<List<CityItem>> GetCitiesAsync();
    public Task<List<MakeItem>> GetMakesAsync();
}
