using System.ComponentModel.DataAnnotations;

namespace NutriTrack.DTO.ProductNutrition;

public class CreateProductCategoryRequest
{
    [Required]
    public string CategoryName { get; set; }
}