using System;
using WebApi.DTOs;
using Contracts.Entities;
using WebApi.Entities.StocksRawData;
using Stocks; // gRPC generated namespace

namespace WebApi.Repositories;

public interface IStocksRepository
{
    public Task<(List<StockRawData> Stocks, int TotalCount)> GetStocksAsync(Filters filters);
    public Task<List<CityItem>> GetCitiesAsync();
    public Task<List<MakeItem>> GetMakesAsync();
}
