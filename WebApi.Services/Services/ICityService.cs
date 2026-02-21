using WebApi.Domain.DTOs;

namespace WebApi.Services;

public interface ICityService
{
    Task<List<CityDto>> GetCitiesAsync();
}