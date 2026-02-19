using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStocksService _stocksService;

        public StocksController(IStocksService stocksService)
        {
            _stocksService = stocksService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<StocksDto>>> GetStocks([FromQuery] FiltersDto filtersDto)
        {
            var (stocks, totalCount) = await _stocksService.GetStocksAsync(filtersDto);

            var response = new PagedResponse<StocksDto>
            {
                Stocks = stocks,
                Page = filtersDto.Page ?? 1,
                TotalCount = totalCount
            };

            response.NextPageUrl = response.GenerateNextPageUrl(Request);

            return Ok(response);
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _stocksService.GetCitiesAsync();
            return Ok(cities);
        }
        [HttpGet("makes")]
        public async Task<IActionResult> GetMakes()
        {
            var makes = await _stocksService.GetMakesAsync();
            return Ok(makes);
        }
    }
}
