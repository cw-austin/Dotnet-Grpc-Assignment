using System;
using WebApi.Domain.DTOs;
using Contracts.Entities;
using WebApi.Domain.Entities.StocksRawData;
using Stocks; // gRPC generated namespace

namespace WebApi.Repository.Repositories;

public interface IStocksRepository
{
    public Task<(List<StockRawData> Stocks, int TotalCount)> GetStocksAsync(Filters filters);
}
