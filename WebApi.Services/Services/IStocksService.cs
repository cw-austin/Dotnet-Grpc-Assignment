using WebApi.Domain.DTOs;

public interface IStocksService
{
    Task<(List<StocksDto> Stocks, int TotalCount)> GetStocksAsync(FiltersDto filtersDto);
}
