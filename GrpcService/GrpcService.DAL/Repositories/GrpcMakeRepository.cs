using Dapper;
using MySqlConnector;
using GrpcService.Entities.MakeItem;
using Microsoft.Extensions.Configuration;

namespace GrpcService.Repositories;

public class GrpcMakeRepository : IGrpcMakeRepository
{
    private readonly string _connectionString;
    public GrpcMakeRepository(IConfiguration configuration) => 
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<List<MakeItem>> GetMakesAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        const string sql = "SELECT makeId, makeName FROM makes ORDER BY makeName ASC";
        return (await connection.QueryAsync<MakeItem>(sql)).ToList();
    }
}