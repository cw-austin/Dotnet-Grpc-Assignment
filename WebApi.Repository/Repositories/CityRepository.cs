using Stocks;
using Google.Protobuf.WellKnownTypes;

namespace WebApi.WebApi.Repository.Repositories;

public class CityRepository : ICityRepository
{
    private readonly CitiesService.CitiesServiceClient _client; 

    public CityRepository(CitiesService.CitiesServiceClient client) 
    {
        _client = client;
    }

    public async Task<List<CityItem>> GetCitiesAsync()
    {
        var response = await _client.GetCitiesAsync(new Empty());
        return response.Cities.ToList();
    }
}