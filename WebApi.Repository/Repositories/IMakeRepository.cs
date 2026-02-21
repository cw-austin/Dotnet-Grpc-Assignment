using Stocks;

namespace WebApi.WebApi.Repository.Repositories;

public interface IMakeRepository
{
    Task<List<MakeItem>> GetMakesAsync();
}