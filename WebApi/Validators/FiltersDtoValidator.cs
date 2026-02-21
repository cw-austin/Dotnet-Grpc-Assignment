using System.ComponentModel.DataAnnotations;
using WebApi.Domain.DTOs;

namespace WebApi.Validators;

public static class FiltersDtoValidator
{
    public static IEnumerable<ValidationResult> Validate(FiltersDto filtersDto)
    {
        if (!string.IsNullOrWhiteSpace(filtersDto.Budget))
        {
            var parts = filtersDto.Budget.Split('-');

            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out var min) ||
                !int.TryParse(parts[1], out var max) ||
                min < 0 || max < 0 || min > max)
            {
                yield return new ValidationResult(
                    "Budget must be in format 'min-max' with non-negative integers and min <= max.",
                    new[] { nameof(FiltersDto.Budget) });
            }
        }

        if (!string.IsNullOrWhiteSpace(filtersDto.Fuel))
        {
            var parts = filtersDto.Fuel.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (!int.TryParse(part, out var value) || value < 0)
                {
                    yield return new ValidationResult(
                        "Fuel must contain non-negative integers separated by '+'.",
                        new[] { nameof(FiltersDto.Fuel) });
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(filtersDto.Cars))
        {
            var parts = filtersDto.Cars.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (!int.TryParse(part, out var value) || value < 0)
                {
                    yield return new ValidationResult(
                        "Cars must contain non-negative integers separated by '+'.",
                        new[] { nameof(FiltersDto.Cars) });
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(filtersDto.Cities))
        {
            var parts = filtersDto.Cities.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (!int.TryParse(part, out var value) || value < 0)
                {
                    yield return new ValidationResult(
                        "Cities must contain non-negative integers separated by '+'.",
                        new[] { nameof(FiltersDto.Cities) });
                }
            }
        }

        var allowedSort = new[]
        {
            "price-asc",
            "price-desc",
            "year-asc",
            "year-desc"
        };

        if (!string.IsNullOrWhiteSpace(filtersDto.SortBy) &&
            !allowedSort.Contains(filtersDto.SortBy.ToLowerInvariant()))
        {
            yield return new ValidationResult(
                "SortBy must be one of: price-asc, price-desc, year-asc, year-desc.",
                new[] { nameof(FiltersDto.SortBy) });
        }
    }
}
