using Dapper;
using Contracts.Entities;

namespace GrpcService.Helpers;

public static class StockQueryBuilder
{
    public static (string Sql, DynamicParameters Parameters) BuildStockQuery(Filters filters)
    {
        var parameters = new DynamicParameters();
        var conditions = new List<string> { "1=1" };

        if (filters.Cars?.Any() == true) { conditions.Add("s.makeId IN @Cars"); parameters.Add("@Cars", filters.Cars); }
        if (filters.Cities?.Any() == true) { conditions.Add("s.cityId IN @Cities"); parameters.Add("@Cities", filters.Cities); }
        if (filters.FuelType?.Any() == true) { conditions.Add("s.fuel IN @FuelType"); parameters.Add("@FuelType", filters.FuelType); }
        if (filters.MinBudget.HasValue) { conditions.Add("s.price >= @MinBudget"); parameters.Add("@MinBudget", filters.MinBudget); }
        if (filters.MaxBudget.HasValue) { conditions.Add("s.price <= @MaxBudget"); parameters.Add("@MaxBudget", filters.MaxBudget); }

        string whereClause = "WHERE " + string.Join(" AND ", conditions);

        int page = filters.Page > 0 ? filters.Page.Value : 1;
        int limit = 20; 
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

        var sql = $@"
            SELECT COUNT(DISTINCT profileId) FROM stocks s {whereClause};

            SELECT s.*, i.imageUrl
            FROM (
                SELECT s.profileId, s.makeYear, s.modelName, s.makeName, s.price, 
                       s.emiText, s.fuel, s.kmsDriven, s.cityName
                FROM stocks s
                {whereClause}
                {orderBy}
                LIMIT @Limit OFFSET @Offset
            ) AS s
            LEFT JOIN stockImages i ON s.profileId = i.profileId
            {orderBy}, s.profileId, i.displayOrder;";

        return (sql, parameters);
    }
}