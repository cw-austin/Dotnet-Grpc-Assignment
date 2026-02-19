using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class FiltersDto : IValidatableObject
    {
        [Microsoft.AspNetCore.Mvc.FromQuery(Name = "budget")]
        public string? Budget { get; set; }

        [Microsoft.AspNetCore.Mvc.FromQuery(Name = "fuel")]
        public string? Fuel { get; set; }

        [Microsoft.AspNetCore.Mvc.FromQuery(Name = "car")]
        public string? Cars { get; set; }

        [Microsoft.AspNetCore.Mvc.FromQuery(Name = "city")]
        public string? Cities { get; set; }

        [Microsoft.AspNetCore.Mvc.FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [Microsoft.AspNetCore.Mvc.FromQuery(Name = "page")]
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int? Page { get; set; } = 1;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 1. Validate Budget format (must be "min-max")
            if (!string.IsNullOrWhiteSpace(Budget))
            {
                var parts = Budget.Split('-');
                if (parts.Length != 2 || 
                    !int.TryParse(parts[0], out var min) || 
                    !int.TryParse(parts[1], out var max) || 
                    min < 0 || max < 0 || min > max)
                {
                    yield return new ValidationResult(
                        "Budget must be in format 'min-max' (e.g., 5-10) where min <= max.",
                        new[] { nameof(Budget) });
                }
            }

            // 2. Validate List Formats (Ensure only digits and plus signs/spaces)
            // This prevents "abc" from reaching the Service
            var listPattern = @"^[0-9\+ ]*$";
            
            if (!string.IsNullOrWhiteSpace(Fuel) && !System.Text.RegularExpressions.Regex.IsMatch(Fuel, listPattern))
                yield return new ValidationResult("Fuel IDs must be numbers separated by '+'.", new[] { nameof(Fuel) });

            if (!string.IsNullOrWhiteSpace(Cars) && !System.Text.RegularExpressions.Regex.IsMatch(Cars, listPattern))
                yield return new ValidationResult("Car IDs must be numbers separated by '+'.", new[] { nameof(Cars) });

            if (!string.IsNullOrWhiteSpace(Cities) && !System.Text.RegularExpressions.Regex.IsMatch(Cities, listPattern))
                yield return new ValidationResult("City IDs must be numbers separated by '+'.", new[] { nameof(Cities) });

            // 3. Validate SortBy options
            var allowedSort = new[] { "price-asc", "price-desc", "year-asc", "year-desc", "default" };
            if (!string.IsNullOrWhiteSpace(SortBy) && !allowedSort.Contains(SortBy.ToLower()))
            {
                yield return new ValidationResult(
                    $"SortBy must be one of: {string.Join(", ", allowedSort)}",
                    new[] { nameof(SortBy) });
            }
        }
    }
}