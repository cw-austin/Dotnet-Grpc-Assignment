using WebApi.DTOs;

public interface IStocksService
{
    Task<(List<StocksDto> Stocks, int TotalCount)> GetStocksAsync(FiltersDto filtersDto);
    Task<List<CityDto>> GetCitiesAsync();
    Task<List<MakeDto>> GetMakesAsync();
}
