using Stocks;

namespace WebApi.WebApi.Repository.Repositories;

public interface ICityRepository
{
    Task<List<CityItem>> GetCitiesAsync();
}