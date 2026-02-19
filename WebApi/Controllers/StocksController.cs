using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;

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
            // Use InputSanitizer for modularized input splitting
            var (minBudget, maxBudget) = WebApi.Helpers.InputSanitizer.ParseBudget(filtersDto.Budget);
            if (minBudget.HasValue) minBudget *= 100000;
            if (maxBudget.HasValue) maxBudget *= 100000;

            var cars = WebApi.Helpers.InputSanitizer.ParseIntList(filtersDto.Cars);
            var cities = WebApi.Helpers.InputSanitizer.ParseIntList(filtersDto.Cities);
            var fuels = WebApi.Helpers.InputSanitizer.ParseIntList(filtersDto.Fuel);

            // Pass sanitized values to service or mapper as needed
            var (stocks, totalCount) = await _stocksService.GetStocksAsync(filtersDto);

            var response = new PagedResponse<StocksDto>
            {
                Stocks = stocks,
                Page = filtersDto.Page ?? 1,
                TotalCount = totalCount
            };

            return Ok(response); // Returns 200 OK with the JSON structure above
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
