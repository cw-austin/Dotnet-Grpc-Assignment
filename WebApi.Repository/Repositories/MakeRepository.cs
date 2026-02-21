using Stocks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.WebApi.Repository.Repositories;

public class MakeRepository : IMakeRepository
{
    // Changed to MakesService to match your updated proto
    private readonly Stocks.MakesService.MakesServiceClient _client;

    public MakeRepository(Stocks.MakesService.MakesServiceClient client)
    {
        _client = client;
    }

    public async Task<List<MakeItem>> GetMakesAsync()
    {
        try
        {
            var response = await _client.GetMakesAsync(new Empty());
            return response.Makes.ToList();
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"gRPC MakesService error: {ex.Status.Detail}");
            throw;
        }
    }
}