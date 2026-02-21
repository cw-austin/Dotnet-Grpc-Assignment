using EntityStocks = GrpcService.Entities.Stocks;
using GrpcService.Entities.StockRow;
namespace GrpcService.Mappers;

public class RepoMapper
{
    public List<EntityStocks> MapToStocks(IEnumerable<StockRow> rows)
    {
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

        return stockDict.Values.ToList();
    }
}