using Grpc.Core;
using Stocks;
using GrpcService.Repositories;
using Google.Protobuf.WellKnownTypes;

namespace GrpcService.Services;
public class GrpcMakeService : MakesService.MakesServiceBase, IGrpcMakeService
{
    private readonly IGrpcMakeRepository _repository;

    public GrpcMakeService(IGrpcMakeRepository repository)
    {
        _repository = repository;
    }

    public override async Task<MakeResponse> GetMakes(Empty request, ServerCallContext context)
    {
        var makes = await _repository.GetMakesAsync();
        var response = new MakeResponse();
        response.Makes.AddRange(makes.Select(m => new Stocks.MakeItem {
            MakeId = m.MakeId,
            MakeName = m.MakeName
        }));
        return response;
    }
}