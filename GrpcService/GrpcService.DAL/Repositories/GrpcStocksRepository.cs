using Contracts.Entities;
using EntityStocks = GrpcService.Entities.Stocks;
using GrpcService.Entities.StockRow;
using GrpcService.Helpers;
using GrpcService.Mappers;
using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace GrpcService.Repositories;

public class GrpcStocksRepository : IGrpcStocksRepository
{
    private readonly string _connectionString;
    private readonly RepoMapper _repoMapper;

    public GrpcStocksRepository(IConfiguration configuration, RepoMapper repoMapper)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("DefaultConnection is missing");
        _repoMapper = repoMapper;
    }

    private MySqlConnection CreateConnection()
        => new MySqlConnection(_connectionString);

    public async Task<(List<EntityStocks> Stocks, int TotalCount)> GetStocksRepository(Filters filters)
    {
        using var connection = CreateConnection();

        var (sql, parameters) = StockQueryBuilder.BuildStockQuery(filters);

        using var multi = await connection.QueryMultipleAsync(sql, parameters);
        var totalCount = await multi.ReadSingleAsync<int>();
        var rows = await multi.ReadAsync<StockRow>();

        var stocks = _repoMapper.MapToStocks(rows);

        return (stocks, totalCount);
    }
}
