using Grpc.Core;
using GrpcService.Repositories;
using Stocks;
using Contracts.Entities;
using Contracts.Enums.FuelType;
using GrpcService.Mappers;

namespace GrpcService.Services;

public class GrpcStocksService : StockService.StockServiceBase, IGrpcStocksService
{
    private readonly IGrpcStocksRepository _grpcStocksRepository;
    private readonly StocksProtoMapper _stocksProtoMapper;

    public GrpcStocksService(
        IGrpcStocksRepository grpcStocksRepository,
        StocksProtoMapper stocksProtoMapper)
    {
        _grpcStocksRepository = grpcStocksRepository;
        _stocksProtoMapper = stocksProtoMapper;
    }

    public override async Task<StocksResponse> GetStocks(
        FiltersRequest request,
        ServerCallContext context)
    {
        try
        {
            // Map FiltersRequest to domain Filters safely
            var filters = MapToDomain(request);

            // Fetch stocks from repository
            var (stocksEntities, totalCount) =
                await _grpcStocksRepository.GetStocksRepository(filters);

            // Map domain entities to proto response
            return _stocksProtoMapper.MapToResponse(stocksEntities, totalCount);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine($"GrpcStocksService.GetStocks exception: {ex}");

            // Return proper gRPC error
            throw new RpcException(
                new Status(StatusCode.Internal, "An error occurred in the gRPC server."),
                ex.Message);
        }
    }

    public override async Task<CityResponse> GetCities(
    Google.Protobuf.WellKnownTypes.Empty request, 
    ServerCallContext context)
    {
        try
        {
            var cityEntities = await _grpcStocksRepository.GetCitiesAsync();

            var response = new CityResponse();
            
            var protoItems = cityEntities.Select(c => new Stocks.CityItem 
            {
                CityId = c.CityId,
                CityName = c.CityName,
                IsPopular = c.IsPopular
            });

            response.Cities.AddRange(protoItems);

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw new RpcException(new Status(StatusCode.Internal, "Internal Error"));
        }
    }

    public override async Task<MakeResponse> GetMakes(
        Google.Protobuf.WellKnownTypes.Empty request, 
        ServerCallContext context)
    {
        try
        {
            var makeEntities = await _grpcStocksRepository.GetMakesAsync();

            var response = new MakeResponse();
            
            var protoItems = makeEntities.Select(m => new Stocks.MakeItem 
            {
                MakeId = m.MakeId,
                MakeName = m.MakeName
            });

            response.Makes.AddRange(protoItems);

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw new RpcException(new Status(StatusCode.Internal, "Internal Error"));
        }
    }

    private static Filters MapToDomain(FiltersRequest request)
    {
        return new Filters
        {
            MinBudget = request.HasMinBudget ? request.MinBudget : null,
            MaxBudget = request.HasMaxBudget ? request.MaxBudget : null,
            SortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "default" : request.SortBy.ToLowerInvariant(),
            Cars = request.Cars?.ToList() ?? new List<int>(),
            Cities = request.Cities?.ToList() ?? new List<int>(),

            FuelType = request.FuelType?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => Enum.TryParse<FuelType>(x, true, out var fuel) ? fuel : (FuelType?)null)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .ToList() ?? new List<FuelType>(),

            Page = request.HasPage ? request.Page : 1
        };
    }
}
