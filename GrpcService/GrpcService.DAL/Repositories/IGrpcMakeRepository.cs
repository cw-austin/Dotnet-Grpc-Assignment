using GrpcService.Entities.MakeItem;

namespace GrpcService.Repositories;

public interface IGrpcMakeRepository
{
    Task<List<MakeItem>> GetMakesAsync();
}