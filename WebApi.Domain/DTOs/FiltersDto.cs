using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Domain.DTOs
{
    public class FiltersDto
    {
        [FromQuery(Name = "budget")]
        public string? Budget { get; set; }

        [FromQuery(Name = "fuel")]
        public string? Fuel { get; set; }

        [FromQuery(Name = "car")]
        public string? Cars { get; set; }

        [FromQuery(Name = "city")]
        public string? Cities { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "page")]
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int? Page { get; set; }
    }
}
