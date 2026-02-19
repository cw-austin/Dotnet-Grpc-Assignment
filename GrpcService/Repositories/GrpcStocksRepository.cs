using Contracts.Entities;
using EntityStocks = GrpcService.Entities.Stocks;
using GrpcService.Entities.CityItem;
using GrpcService.Entities.MakeItem;
using GrpcService.Entities.StockRow;
using MySqlConnector;
using Dapper;

namespace GrpcService.Repositories;

public class GrpcStocksRepository : IGrpcStocksRepository
{
    private readonly string _connectionString;

    public GrpcStocksRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("DefaultConnection is missing");
    }

    private MySqlConnection CreateConnection()
        => new MySqlConnection(_connectionString);

    public async Task<(List<EntityStocks> Stocks, int TotalCount)> GetStocksRepository(Filters filters)
    {
        using var connection = CreateConnection();

        var whereClause = " WHERE 1=1";
        var parameters = new DynamicParameters();

        // 1. Build the Where Clause
        if (filters.Cars?.Any() == true) { whereClause += " AND s.makeId IN @Cars"; parameters.Add("@Cars", filters.Cars); }
        if (filters.Cities?.Any() == true) { whereClause += " AND s.cityId IN @Cities"; parameters.Add("@Cities", filters.Cities); }
        if (filters.FuelType?.Any() == true) { whereClause += " AND s.fuel IN @FuelType"; parameters.Add("@FuelType", filters.FuelType); }
        if (filters.MinBudget.HasValue) { whereClause += " AND s.price >= @MinBudget"; parameters.Add("@MinBudget", filters.MinBudget); }
        if (filters.MaxBudget.HasValue) { whereClause += " AND s.price <= @MaxBudget"; parameters.Add("@MaxBudget", filters.MaxBudget); }

        // 2. Pagination setup
        int page = filters.Page > 0 ? filters.Page.Value : 1;
        int limit = 20; // Change this to a very large number if you truly want "ALL" data
        int offset = (page - 1) * limit;
        parameters.Add("@Limit", limit);
        parameters.Add("@Offset", offset);

        var orderBy = filters.SortBy switch
        {
            "price-asc" => "ORDER BY s.price ASC",
            "price-desc" => "ORDER BY s.price DESC",
            "year-asc" => "ORDER BY s.makeYear ASC",
            "year-desc" => "ORDER BY s.makeYear DESC",
            _ => "ORDER BY s.profileId"
        };

        // 3. THE FIX: Use a Subquery for the Stocks so LIMIT applies to CARS, not ROWS
        var sql = $@"
        SELECT COUNT(DISTINCT profileId) FROM stocks s {whereClause};

        SELECT s.*, i.imageUrl
        FROM (
            SELECT s.profileId, s.makeYear, s.modelName, s.makeName, s.price, 
                s.emiText, s.fuel, s.kmsDriven, s.cityName
            FROM stocks s
            {whereClause}
            {orderBy} -- Correctly sorts the subset of 20 cars
            LIMIT @Limit OFFSET @Offset
        ) AS s
        LEFT JOIN stockImages i ON s.profileId = i.profileId
        {orderBy}, s.profileId, i.displayOrder;"; // Apply same sort here + group images

        using var multi = await connection.QueryMultipleAsync(sql, parameters);
        var totalCount = await multi.ReadSingleAsync<int>();
        var rows = (await multi.ReadAsync<StockRow>()).ToList();

        var stockDict = new Dictionary<string, EntityStocks>();

        foreach (var row in rows)
        {
            if (!stockDict.TryGetValue(row.ProfileId!, out var stock))
            {
                stock = new EntityStocks
                {
                    ProfileId = row.ProfileId,
                    MakeYear = row.MakeYear,
                    MakeName = row.MakeName,
                    ModelName = row.ModelName,
                    Price = row.Price,
                    EmiText = row.EmiText,
                    Fuel = row.Fuel,
                    KmsDriven = row.KmsDriven,
                    CityName = row.CityName,
                    ImageUrls = new List<string>()
                };
                stockDict[row.ProfileId!] = stock;
            }

            if (!string.IsNullOrEmpty(row.ImageUrl))
            {
                stock.ImageUrls.Add(row.ImageUrl);
            }
        }

        return (stockDict.Values.ToList(), totalCount);
    }
    public async Task<List<CityItem>> GetCitiesAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT cityId, cityName, isPopular FROM cities ORDER BY cityName ASC";
        var cities = await connection.QueryAsync<CityItem>(sql);
        return cities.ToList();
    }

    public async Task<List<MakeItem>> GetMakesAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT makeId, makeName FROM makes ORDER BY makeName ASC";
        var makes = await connection.QueryAsync<MakeItem>(sql);
        return makes.ToList();
    }
}
