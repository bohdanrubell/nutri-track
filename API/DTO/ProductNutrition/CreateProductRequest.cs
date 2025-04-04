﻿namespace NutriTrack.DTO.ProductNutrition;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string CategoryName { get; set; }
    public int CaloriesPer100Grams { get; set; }
    public int ProteinPer100Grams { get; set; }
    public int FatPer100Grams { get; set; }
    public int CarbohydratesPer100Grams { get; set; }

    public IFormFile? File { get; set; }
}