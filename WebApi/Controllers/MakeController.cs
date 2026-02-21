using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MakeController: ControllerBase
    {
        private readonly IMakeService _makeService;
        public MakeController(IMakeService makeService)
        {
            _makeService = makeService;
        }
        [HttpGet("makes")]
        public async Task<IActionResult> GetMakes()
        {
            try
            {
                var makes = await _makeService.GetMakesAsync();
                return Ok(makes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMakes: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}