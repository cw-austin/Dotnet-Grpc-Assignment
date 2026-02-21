
using WebApi.Domain.DTOs;
using WebApi.Mappers;
using WebApi.Repository.Repositories;

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
        
        var (entities, totalCount) = await _stocksRepository.GetStocksAsync(filters);

        var dtos = _stocksDtoMapper.Map(entities);

        return (dtos, totalCount);
    }
}
