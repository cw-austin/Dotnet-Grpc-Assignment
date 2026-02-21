using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Enums.FuelType;

namespace Contracts.Entities;

public class Filters
{
    [Range(0, int.MaxValue, ErrorMessage = "MinBudget must be a non-negative integer.")]
    public int? MinBudget { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "MaxBudget must be a non-negative integer.")]
    public int? MaxBudget { get; set; }
    public string? SortBy { get; set; } = "default";
    public List<int>? Cars { get; set; }
    public List<FuelType>? FuelType { get; set; }
    public List<int>? Cities { get; set; }
    public int? Page { get; set; }
}