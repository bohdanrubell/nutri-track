using Microsoft.AspNetCore.Identity;
using NutriTrack.Entities;

namespace NutriTrack.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context, UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new User
            {
                UserName = "banan",
                Email = "banan@gmail.com"
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "User");
        }

        if (context.ActivityLevels.Any() && context.GoalTypes.Any()) return;

        var activityLevels = new List<ActivityLevel>
        {
            new()
            {
                Name = "Sedentary",
                Ratio = 1200
            },
            new()
            {
                Name = "Light",
                Ratio = 1375
            },
            new()
            {
                Name = "Moderate",
                Ratio = 1550
            },
            new()
            {
                Name = "High",
                Ratio = 1725
            },
            new()
            {
                Name = "VeryHigh",
                Ratio = 1900
            }
        };

        await context.ActivityLevels.AddRangeAsync(activityLevels);

        var goals = new List<GoalType>
        {
            new() { Name = "WeightLoss", Percent = 85 },
            new() { Name = "WeightMaintenance", Percent = 100 },
            new() { Name = "WeightGain", Percent = 115 }
        };
        
        await context.GoalTypes.AddRangeAsync(goals);

        if (context.ProductNutritionCategories.Any() && context.ProductNutritions.Any()) return;
        
        var categories = new List<ProductNutritionCategory>
        {
            new() { Id = 1, Name = "Fruits" },
            new() { Id = 2, Name = "Vegetables" },
            new() { Id = 3, Name = "Dairy" },
            new() { Id = 4, Name = "Meat" }
        };

        await context.ProductNutritionCategories.AddRangeAsync(categories);
        
        var products = new List<ProductNutrition>
        {
            // Продукти категорії "Fruits"
            new()
            {
                Name = "Apple", CaloriesPer100Grams = 52, ProteinPer100Grams = 0, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 14, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Banana", CaloriesPer100Grams = 96, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 23, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Orange", CaloriesPer100Grams = 47, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 12, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Strawberry", CaloriesPer100Grams = 32, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 8, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Grapes", CaloriesPer100Grams = 69, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 18, ProductNutritionCategoryId = 1
            },

            // Продукти категорії "Vegetables"
            new()
            {
                Name = "Carrot", CaloriesPer100Grams = 41, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 10, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Broccoli", CaloriesPer100Grams = 55, ProteinPer100Grams = 4, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 11, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Tomato", CaloriesPer100Grams = 18, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Spinach", CaloriesPer100Grams = 23, ProteinPer100Grams = 3, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Cucumber", CaloriesPer100Grams = 16, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },

            // Продукти категорії "Dairy"
            new()
            {
                Name = "Milk", CaloriesPer100Grams = 42, ProteinPer100Grams = 3, FatPer100Grams = 1,
                CarbohydratesPer100Grams = 5, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Yogurt", CaloriesPer100Grams = 59, ProteinPer100Grams = 3, FatPer100Grams = 2,
                CarbohydratesPer100Grams = 7, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Cheese", CaloriesPer100Grams = 402, ProteinPer100Grams = 25, FatPer100Grams = 33,
                CarbohydratesPer100Grams = 1, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Butter", CaloriesPer100Grams = 717, ProteinPer100Grams = 1, FatPer100Grams = 81,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Cottage Cheese", CaloriesPer100Grams = 98, ProteinPer100Grams = 11, FatPer100Grams = 4,
                CarbohydratesPer100Grams = 3, ProductNutritionCategoryId = 3
            },

            // Продукти категорії "Meat"
            new()
            {
                Name = "Chicken Breast", CaloriesPer100Grams = 165, ProteinPer100Grams = 31, FatPer100Grams = 4,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Beef", CaloriesPer100Grams = 250, ProteinPer100Grams = 26, FatPer100Grams = 15,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Pork", CaloriesPer100Grams = 242, ProteinPer100Grams = 27, FatPer100Grams = 14,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Turkey", CaloriesPer100Grams = 189, ProteinPer100Grams = 29, FatPer100Grams = 7,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Lamb", CaloriesPer100Grams = 294, ProteinPer100Grams = 25, FatPer100Grams = 21,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            }
        };
        
        await context.ProductNutritions.AddRangeAsync(products);
        
        await context.SaveChangesAsync();
    }
}