﻿namespace NutriTrack.Entities;

public class ProductRecord
{
    public int Id { get; set; }
    public double Grams { get; set; }
    
    public int ProductNutritionId { get; set; }
    public ProductNutrition ProductNutrition { get; set; }
    
    public int RecordId { get; set; }
    public Record Record { get; set; }
}