using NutriTrack.Entities;

namespace NutriTrack.Extensions;

public static class ProductNutritionExtensions
{
    public static IQueryable<ProductNutrition> Sort(this IQueryable<ProductNutrition> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);

        query = orderBy switch
        {
            "calories" => query.OrderBy(p => p.CaloriesPer100Grams),
            "caloriesDesc" => query.OrderByDescending(p => p.CaloriesPer100Grams),
            "protein" => query.OrderBy(p => p.ProteinPer100Grams),
            "proteinDesc" => query.OrderByDescending(p => p.ProteinPer100Grams),
            "fat" => query.OrderBy(p => p.FatPer100Grams),
            "fatDesc" => query.OrderByDescending(p => p.FatPer100Grams),
            "carbohydrates" => query.OrderBy(p => p.CarbohydratesPer100Grams),
            "carbohydratesDesc" => query.OrderByDescending(p => p.CarbohydratesPer100Grams),
            _ => query.OrderBy(n => n.Name)
        };

        return query;
    }

    public static IQueryable<ProductNutrition> Search(this IQueryable<ProductNutrition> query, string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return query;

        var lowerCaseSearchTerm = searchTerm.Trim();

        return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm.ToLower()));
    }
    
    public static IQueryable<ProductNutrition> FilterByCategories(this IQueryable<ProductNutrition> query, string? categories)
    {
        var typeList = new List<string>();

        if (!string.IsNullOrEmpty(categories)) typeList.AddRange(categories.Split(",").ToList());

        if (typeList.Count > 0) query = query.Where(p => typeList.Contains(p.ProductNutritionCategory.Name));

        return query;
    }
}