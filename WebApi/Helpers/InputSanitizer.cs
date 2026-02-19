using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Helpers
{
    public static class InputSanitizer
    {
        public static (int? min, int? max) ParseBudget(string? budget)
        {
            if (!string.IsNullOrWhiteSpace(budget))
            {
                var parts = budget.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out var min) && int.TryParse(parts[1], out var max))
                {
                    return (min, max);
                }
            }
            return (null, null);
        }

        public static List<int>? ParseIntList(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return value.Split(new[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x, out var val) ? val : (int?)null)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .ToList();
        }
    }
}
