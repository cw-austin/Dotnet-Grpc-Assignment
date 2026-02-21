using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CityController: ControllerBase
    {
        private readonly ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }
        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var cities = await _cityService.GetCitiesAsync();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCities: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}