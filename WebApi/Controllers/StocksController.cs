using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Domain.DTOs;
using WebApi.Domain.Entities;
using WebApi.Validators;

namespace WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStocksService _stocksService;
        private readonly ICityService _cityService;
        private readonly IMakeService _makeService;

        public StocksController(IStocksService stocksService, ICityService cityService, IMakeService makeService)
        {
            _stocksService = stocksService;
            _cityService = cityService;
            _makeService = makeService;
        }

        [HttpGet("stocks")]
        public async Task<ActionResult<PagedResponse<StocksDto>>> GetStocks([FromQuery] FiltersDto filtersDto)
        {
            try{
                var validationResults = FiltersDtoValidator.Validate(filtersDto).ToList();
                if (validationResults.Count > 0)
                {
                    foreach (var validationResult in validationResults)
                    {
                        var memberName = validationResult.MemberNames.FirstOrDefault() ?? string.Empty;
                        ModelState.AddModelError(memberName, validationResult.ErrorMessage ?? "Invalid filter value.");
                    }

                    return ValidationProblem(ModelState);
                }

                var (stocks, totalCount) = await _stocksService.GetStocksAsync(filtersDto);

                var response = new PagedResponse<StocksDto>
                {
                    Stocks = stocks,
                    Page = filtersDto.Page ?? 1,
                    PageSize = 20,
                    TotalCount = totalCount
                };

                return Ok(response); // Returns 200 OK with the JSON structure
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in GetStocks: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}