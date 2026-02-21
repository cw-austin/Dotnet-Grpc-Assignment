

using Contracts.Entities;
using Contracts.Enums.FuelType;
using Riok.Mapperly.Abstractions;
using WebApi.Domain.DTOs;

namespace WebApi.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class FiltersMapper
{
    [MapperIgnoreTarget(nameof(Filters.MinBudget))]
    [MapperIgnoreTarget(nameof(Filters.MaxBudget))]
    [MapperIgnoreTarget(nameof(Filters.Cars))]
    [MapperIgnoreTarget(nameof(Filters.Cities))]
    [MapperIgnoreTarget(nameof(Filters.FuelType))]
    [MapperIgnoreTarget(nameof(Filters.SortBy))]
    [MapperIgnoreTarget(nameof(Filters.Page))]
    public partial Filters Map(FiltersDto dto);

    public Filters MapWithLogic(FiltersDto dto)
    {
        var entity = Map(dto);
        ApplyCustomLogic(dto, entity);
        return entity;
    }

    private static void ApplyCustomLogic(FiltersDto dto, Filters entity)
    {
        if (!string.IsNullOrWhiteSpace(dto.Budget))
        {
            var parts = dto.Budget.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                entity.MinBudget = int.TryParse(parts[0], out var min) ? min * 100000 : null;
                entity.MaxBudget = int.TryParse(parts[1], out var max) ? max * 100000 : null;
            }
        }

        entity.Cars = ParseIntList(dto.Cars);
        entity.Cities = ParseIntList(dto.Cities);
        entity.FuelType = ParseFuelList(dto.Fuel);

        entity.SortBy = string.IsNullOrWhiteSpace(dto.SortBy)
            ? "default"
            : dto.SortBy.ToLowerInvariant();

        entity.Page = dto.Page ?? 1;
    }

    private static List<int>? ParseIntList(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        return value
            .Split(new[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.TryParse(x, out var val) ? val : (int?)null)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .ToList();
    }

    private static List<FuelType>? ParseFuelList(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value
            .Split('+', StringSplitOptions.RemoveEmptyEntries)
            .Select(x =>
                Enum.TryParse<FuelType>(x, true, out var fuel)
                    ? fuel
                    : (FuelType?)null)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .ToList();
    }
}
