using WebApi.Domain.DTOs;

namespace WebApi.Services;

public interface IMakeService
{
    Task<List<MakeDto>> GetMakesAsync();
}