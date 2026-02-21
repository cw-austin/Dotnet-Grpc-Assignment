using Stocks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace GrpcService.Services;

public interface IGrpcMakeService
{
    Task<MakeResponse> GetMakes(Empty request, ServerCallContext context);
}