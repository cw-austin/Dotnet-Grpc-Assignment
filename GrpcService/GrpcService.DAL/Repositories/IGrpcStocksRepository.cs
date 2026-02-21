using System;
namespace GrpcService.Repositories;
using EntityStocks = GrpcService.Entities.Stocks;
using Contracts.Entities;

public interface IGrpcStocksRepository
{
    public Task<(List<EntityStocks> Stocks, int TotalCount)> GetStocksRepository(Filters filters);
}
