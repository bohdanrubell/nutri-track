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
            
            var admin = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (context.ActivityLevels.Any() && context.GoalTypes.Any()) return;

        var activityLevels = new List<ActivityLevel>
        {
            new()
            {
                Name = "Сидяча",
                Ratio = 1200
            },
            new()
            {
                Name = "Легка",
                Ratio = 1375
            },
            new()
            {
                Name = "Помірна",
                Ratio = 1550
            },
            new()
            {
                Name = "Висока",
                Ratio = 1725
            },
            new()
            {
                Name = "Дуже висока",
                Ratio = 1900
            }
        };

        await context.ActivityLevels.AddRangeAsync(activityLevels);

        var goals = new List<GoalType>
        {
            new() { Name = "Схуднення", Percent = 85 },
            new() { Name = "Підтримка ваги", Percent = 100 },
            new() { Name = "Набір ваги", Percent = 115 }
        };
        
        await context.GoalTypes.AddRangeAsync(goals);

        if (context.ProductNutritionCategories.Any() && context.ProductNutritions.Any()) return;
        
        var categories = new List<ProductNutritionCategory>
        {
            new() { Id = 1, Name = "Фрукти" },
            new() { Id = 2, Name = "Овочі" },
            new() { Id = 3, Name = "Молочні продукти" },
            new() { Id = 4, Name = "М'ясо" }
        };

        await context.ProductNutritionCategories.AddRangeAsync(categories);
        
        var products = new List<ProductNutrition>
        {
            // Продукти категорії "Fruits"
            new()
            {
                Name = "Яблуко", CaloriesPer100Grams = 52, ProteinPer100Grams = 0, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 14, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Банан", CaloriesPer100Grams = 96, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 23, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Апельсин", CaloriesPer100Grams = 47, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 12, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Полуниця", CaloriesPer100Grams = 32, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 8, ProductNutritionCategoryId = 1
            },
            new()
            {
                Name = "Виноград", CaloriesPer100Grams = 69, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 18, ProductNutritionCategoryId = 1
            },

            // Продукти категорії "Vegetables"
            new()
            {
                Name = "Морковка", CaloriesPer100Grams = 41, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 10, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Броколі", CaloriesPer100Grams = 55, ProteinPer100Grams = 4, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 11, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Помідор", CaloriesPer100Grams = 18, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Шпинат", CaloriesPer100Grams = 23, ProteinPer100Grams = 3, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },
            new()
            {
                Name = "Огірок", CaloriesPer100Grams = 16, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },

            // Продукти категорії "Dairy"
            new()
            {
                Name = "Молоко", CaloriesPer100Grams = 42, ProteinPer100Grams = 3, FatPer100Grams = 1,
                CarbohydratesPer100Grams = 5, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Йогурт", CaloriesPer100Grams = 59, ProteinPer100Grams = 3, FatPer100Grams = 2,
                CarbohydratesPer100Grams = 7, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Сир", CaloriesPer100Grams = 402, ProteinPer100Grams = 25, FatPer100Grams = 33,
                CarbohydratesPer100Grams = 1, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Масло", CaloriesPer100Grams = 717, ProteinPer100Grams = 1, FatPer100Grams = 81,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 3
            },
            new()
            {
                Name = "Сир кисломолочий", CaloriesPer100Grams = 98, ProteinPer100Grams = 11, FatPer100Grams = 4,
                CarbohydratesPer100Grams = 3, ProductNutritionCategoryId = 3
            },

            // Продукти категорії "Meat"
            new()
            {
                Name = "Курине філе", CaloriesPer100Grams = 165, ProteinPer100Grams = 31, FatPer100Grams = 4,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Яловичина", CaloriesPer100Grams = 250, ProteinPer100Grams = 26, FatPer100Grams = 15,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Свинячий стейк", CaloriesPer100Grams = 242, ProteinPer100Grams = 27, FatPer100Grams = 14,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Індичка", CaloriesPer100Grams = 189, ProteinPer100Grams = 29, FatPer100Grams = 7,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new()
            {
                Name = "Каре ягня", CaloriesPer100Grams = 294, ProteinPer100Grams = 25, FatPer100Grams = 21,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            }
        };
        
        await context.ProductNutritions.AddRangeAsync(products);
        
        await context.SaveChangesAsync();
    }
}