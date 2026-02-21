using GrpcService.Entities.CityItem;

namespace GrpcService.Repositories;

public interface IGrpcCityRepository
{
    Task<List<CityItem>> GetCitiesAsync();
}