using Grpc.Core;
using Google.Protobuf.WellKnownTypes; // Correct: Use the namespace, not the type
using GrpcService.Repositories;
using Stocks;

namespace GrpcService.Services; 

public class GrpcCityService : CitiesService.CitiesServiceBase, IGrpcCityService
{
    private readonly IGrpcCityRepository _repository;

    public GrpcCityService(IGrpcCityRepository repository)
    {
        _repository = repository;
    }

    public override async Task<CityResponse> GetCities(Empty request, ServerCallContext context)
    {
        var cities = await _repository.GetCitiesAsync();
        var response = new CityResponse();
        
        response.Cities.AddRange(cities.Select(c => new Stocks.CityItem {
            CityId = c.CityId,
            CityName = c.CityName,
            IsPopular = c.IsPopular
        }));
        
        return response;
    }
}