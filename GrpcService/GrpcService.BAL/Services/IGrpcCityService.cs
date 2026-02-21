using Stocks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace GrpcService.Services;

public interface IGrpcCityService
{
    Task<CityResponse> GetCities(Empty request, ServerCallContext context);
}