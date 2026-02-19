using Contracts.Entities;
using Contracts.Enums.FuelType;
using Riok.Mapperly.Abstractions;
using WebApi.DTOs;

namespace WebApi.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class FiltersMapper
{
    // Let Mapperly handle simple primitive matches automatically (like Page)
    // We only ignore the ones that require complex string parsing
    [MapperIgnoreTarget(nameof(Filters.MinBudget))]
    [MapperIgnoreTarget(nameof(Filters.MaxBudget))]
    [MapperIgnoreTarget(nameof(Filters.Cars))]
    [MapperIgnoreTarget(nameof(Filters.Cities))]
    [MapperIgnoreTarget(nameof(Filters.FuelType))]
    public partial Filters MapDtoToEntity(FiltersDto dto);

    public Filters MapWithLogic(FiltersDto dto)
    {
        var entity = MapDtoToEntity(dto);
        
        // 1. Handle Budget (Lakh to Absolute conversion)
        var (min, max) = Helpers.InputSanitizer.ParseBudget(dto.Budget);
        entity.MinBudget = min.HasValue ? min * 100_000 : null;
        entity.MaxBudget = max.HasValue ? max * 100_000 : null;

        // 2. Handle Lists using your centralized Sanitizer
        entity.Cars = Helpers.InputSanitizer.ParseIntList(dto.Cars);
        entity.Cities = Helpers.InputSanitizer.ParseIntList(dto.Cities);

        // 3. Handle Enums specifically
        entity.FuelType = ParseFuelList(dto.Fuel);

        // 4. Normalize SortBy
        entity.SortBy = string.IsNullOrWhiteSpace(dto.SortBy)
            ? "default"
            : dto.SortBy.ToLowerInvariant();

        return entity;
    }

    private static List<FuelType>? ParseFuelList(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        // Using + and space for consistency with other list parsers
        return value
            .Split(new[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Enum.TryParse<FuelType>(x, true, out var fuel) ? fuel : (FuelType?)null)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .ToList();
    }
}
