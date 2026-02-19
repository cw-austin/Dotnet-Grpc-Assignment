namespace WebApi.DTOs;

public record CityDto(
    int CityId, 
    string CityName, 
    bool IsPopular
);