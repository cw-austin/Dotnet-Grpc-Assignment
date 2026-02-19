using WebApi.DTOs;
using WebApi.Repositories;
using WebApi.Entities.StocksRawData;
using WebApi.Mappers;

namespace WebApi.Services;

public class StocksService : IStocksService
{
    private readonly IStocksRepository _stocksRepository;
    private readonly FiltersMapper _filtersMapper;
    private readonly MetadataMapper _metadataMapper;
    private readonly StocksDtoMapper _stocksDtoMapper;

    public StocksService(
        IStocksRepository stocksRepository,
        FiltersMapper filtersMapper,
        MetadataMapper metadataMapper,
        StocksDtoMapper stocksDtoMapper)
    {
        _stocksRepository = stocksRepository;
        _filtersMapper = filtersMapper;
        _metadataMapper = metadataMapper;
        _stocksDtoMapper = stocksDtoMapper;
    }

    public async Task<(List<StocksDto> Stocks, int TotalCount)> GetStocksAsync(FiltersDto filtersDto)
    {
        var filters = _filtersMapper.MapWithLogic(filtersDto);
        
        //deconstructure the tuple returned by the repository into separate variables
        var (entities, totalCount) = await _stocksRepository.GetStocksAsync(filters);

        var dtos = _stocksDtoMapper.Map(entities);

        return (dtos, totalCount);
    }
    public async Task<List<CityDto>> GetCitiesAsync()
    {
        var items = await _stocksRepository.GetCitiesAsync();
        return _metadataMapper.MapToCityDtos(items);
    }

    public async Task<List<MakeDto>> GetMakesAsync()
    {
        var items = await _stocksRepository.GetMakesAsync();
        return _metadataMapper.MapToMakeDtos(items);
    }
}
