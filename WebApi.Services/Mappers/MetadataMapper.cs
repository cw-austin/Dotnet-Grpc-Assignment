using Riok.Mapperly.Abstractions;
using WebApi.Domain.DTOs;
using Stocks; // This is the namespace for gRPC generated code

namespace WebApi.Mappers;

[Mapper]
public partial class MetadataMapper
{
    // Individual object mapping
    public partial CityDto MapToDto(CityItem source);
    
    // Collection mapping
    public partial List<CityDto> MapToCityDtos(List<CityItem> source);

    // Individual object mapping
    public partial MakeDto MapToDto(MakeItem source);

    // Collection mapping
    public partial List<MakeDto> MapToMakeDtos(List<MakeItem> source);
}