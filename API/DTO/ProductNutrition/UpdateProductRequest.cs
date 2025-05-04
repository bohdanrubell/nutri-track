using System.ComponentModel.DataAnnotations;

namespace NutriTrack.DTO.ProductNutrition;

public class UpdateProductRequest
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string CategoryName { get; set; }
    [Required]
    public decimal CaloriesPer100Grams { get; set; }
    [Required]
    public decimal ProteinPer100Grams { get; set; }
    [Required]
    public decimal FatPer100Grams { get; set; }
    [Required]
    public decimal CarbohydratesPer100Grams { get; set; }

    public IFormFile? File { get; set; }
}