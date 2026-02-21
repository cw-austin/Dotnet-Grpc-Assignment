namespace GrpcService.Entities.CityItem;

public class CityItem
{
    public int CityId { get; set; }

    public string CityName { get; set; } = string.Empty;

    public bool IsPopular { get; set; }
}