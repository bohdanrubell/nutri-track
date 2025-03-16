﻿using System.ComponentModel.DataAnnotations;

namespace NutriTrack.DTO;

public class UpdateProductNutritionRequest
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string CategoryName { get; set; }
    [Required]
    public int CaloriesPer100Grams { get; set; }
    [Required]
    public int ProteinPer100Grams { get; set; }
    [Required]
    public int FatPer100Grams { get; set; }
    [Required]
    public int CarbohydratesPer100Grams { get; set; }
}