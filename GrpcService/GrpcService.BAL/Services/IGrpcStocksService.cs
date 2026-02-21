using System;

namespace GrpcService.Services;

using System.Threading.Tasks;
using Contracts.Entities;
using Grpc.Core;
using Stocks;

public interface IGrpcStocksService
{
	Task<StocksResponse> GetStocks(FiltersRequest request, ServerCallContext context);
}
