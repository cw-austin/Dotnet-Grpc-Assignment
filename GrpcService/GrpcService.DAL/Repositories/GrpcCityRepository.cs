using Dapper;
using MySqlConnector;
using GrpcService.Entities.CityItem;
using Microsoft.Extensions.Configuration;

namespace GrpcService.Repositories;

public class GrpcCityRepository : IGrpcCityRepository
{
    private readonly string _connectionString;
    public GrpcCityRepository(IConfiguration configuration) => 
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<List<CityItem>> GetCitiesAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        const string sql = "SELECT cityId, cityName, isPopular FROM cities ORDER BY cityName ASC";
        return (await connection.QueryAsync<CityItem>(sql)).ToList();
    }
}