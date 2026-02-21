

using WebApi.Domain.DTOs;
using WebApi.Mappers;
using WebApi.WebApi.Repository.Repositories;

namespace WebApi.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly MetadataMapper _mapper;

    public CityService(ICityRepository cityRepository, MetadataMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<List<CityDto>> GetCitiesAsync()
    {
        var entities = await _cityRepository.GetCitiesAsync();
        
        return _mapper.MapToCityDtos(entities);
    }
}