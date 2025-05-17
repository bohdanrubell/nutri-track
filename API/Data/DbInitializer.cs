using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Entities;

namespace NutriTrack.Data;

public static class DbInitializer
{
    public static async Task InitializeDataBase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
                      ?? throw new InvalidOperationException("Failed to retrieve application context");
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>()
                          ?? throw new InvalidOperationException("Failed to retrieve user manager");

        await InitializeData(context, userManager);
    }
    private static async Task InitializeData(ApplicationDbContext context, UserManager<User> userManager)
    {
        await context.Database.MigrateAsync();
        
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
        
        if (!context.ProductNutritions.Any())
        {
            var products = new List<ProductNutrition>
            {
                // Фрукти (1)
                new() { Name = "Яблуко", CaloriesPer100Grams = 52, ProteinPer100Grams = 0, FatPer100Grams = 0, CarbohydratesPer100Grams = 14, ProductNutritionCategoryId = 1 },
                new() { Name = "Банан", CaloriesPer100Grams = 96, ProteinPer100Grams = 1, FatPer100Grams = 0, CarbohydratesPer100Grams = 23, ProductNutritionCategoryId = 1 },
                new() { Name = "Апельсин", CaloriesPer100Grams = 47, ProteinPer100Grams = 1, FatPer100Grams = 0, CarbohydratesPer100Grams = 12, ProductNutritionCategoryId = 1 },
                new() { Name = "Полуниця", CaloriesPer100Grams = 32, ProteinPer100Grams = 1, FatPer100Grams = 0, CarbohydratesPer100Grams = 8, ProductNutritionCategoryId = 1 },
                new() { Name = "Виноград", CaloriesPer100Grams = 69, ProteinPer100Grams = 1, FatPer100Grams = 0, CarbohydratesPer100Grams = 18, ProductNutritionCategoryId = 1 },

                // Овочі (2)
                new() { Name = "Морковка", CaloriesPer100Grams = 41, ProteinPer100Grams = 1, FatPer100Grams = 0, CarbohydratesPer100Grams = 10, ProductNutritionCategoryId = 2 },
                new() { Name = "Броколі", CaloriesPer100Grams = 55, ProteinPer100Grams = 4, FatPer100Grams = 0, CarbohydratesPer100Grams = 11, ProductNutritionCategoryId = 2 },
                new() { Name = "Помідор", CaloriesPer100Grams = 18, ProteinPer100Grams = 1, FatPer100Grams = 0, CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2 },
                new() { Name = "Шпинат", CaloriesPer100Grams = 23, ProteinPer100Grams = 3, FatPer100Grams = 0, CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2 },
                new() { Name = "Огірок", CaloriesPer100Grams = 16, ProteinPer100Grams = 1, FatPer100Grams = 0, CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2 },

                // Молочні (3)
                new() { Name = "Молоко", CaloriesPer100Grams = 42, ProteinPer100Grams = 3, FatPer100Grams = 1, CarbohydratesPer100Grams = 5, ProductNutritionCategoryId = 3 },
                new() { Name = "Йогурт", CaloriesPer100Grams = 59, ProteinPer100Grams = 3, FatPer100Grams = 2, CarbohydratesPer100Grams = 7, ProductNutritionCategoryId = 3 },
                new() { Name = "Сир", CaloriesPer100Grams = 402, ProteinPer100Grams = 25, FatPer100Grams = 33, CarbohydratesPer100Grams = 1, ProductNutritionCategoryId = 3 },
                new() { Name = "Масло", CaloriesPer100Grams = 717, ProteinPer100Grams = 1, FatPer100Grams = 81, CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 3 },
                new() { Name = "Сир кисломолочий", CaloriesPer100Grams = 98, ProteinPer100Grams = 11, FatPer100Grams = 4, CarbohydratesPer100Grams = 3, ProductNutritionCategoryId = 3 },

                // М’ясо (4)
                new() { Name = "Курине філе", CaloriesPer100Grams = 165, ProteinPer100Grams = 31, FatPer100Grams = 4, CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4 },
                new() { Name = "Яловичина", CaloriesPer100Grams = 250, ProteinPer100Grams = 26, FatPer100Grams = 15, CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4 },
                new() { Name = "Свинячий стейк", CaloriesPer100Grams = 242, ProteinPer100Grams = 27, FatPer100Grams = 14, CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4 },
                new() { Name = "Індичка", CaloriesPer100Grams = 189, ProteinPer100Grams = 29, FatPer100Grams = 7, CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4 },
                new() { Name = "Каре ягня", CaloriesPer100Grams = 294, ProteinPer100Grams = 25, FatPer100Grams = 21, CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4 },
                
                // Зернові та хліб (5)
                new() { Name = "Вівсянка", CaloriesPer100Grams = 68, ProteinPer100Grams = 2.4m, FatPer100Grams = 1.4m, CarbohydratesPer100Grams = 12, ProductNutritionCategoryId = 5 },
                new() { Name = "Хліб цільнозерновий", CaloriesPer100Grams = 247, ProteinPer100Grams = 8.5m, FatPer100Grams = 4.2m, CarbohydratesPer100Grams = 41, ProductNutritionCategoryId = 5 },

                // Крупи (6)
                new() { Name = "Гречка варена", CaloriesPer100Grams = 110, ProteinPer100Grams = 4.2m, FatPer100Grams = 1.3m, CarbohydratesPer100Grams = 20.5m, ProductNutritionCategoryId = 6 },
                new() { Name = "Рис білий варений", CaloriesPer100Grams = 130, ProteinPer100Grams = 2.4m, FatPer100Grams = 0.2m, CarbohydratesPer100Grams = 28.2m, ProductNutritionCategoryId = 6 },

                // Напої (7)
                new() { Name = "Чорна кава", CaloriesPer100Grams = 1, ProteinPer100Grams = 0, FatPer100Grams = 0, CarbohydratesPer100Grams = 0.3m, ProductNutritionCategoryId = 7 },
                new() { Name = "Кола", CaloriesPer100Grams = 42, ProteinPer100Grams = 0, FatPer100Grams = 0, CarbohydratesPer100Grams = 10.6m, ProductNutritionCategoryId = 7 },
                new() { Name = "Апельсиновий сік", CaloriesPer100Grams = 45, ProteinPer100Grams = 0.7m, FatPer100Grams = 0.2m, CarbohydratesPer100Grams = 10.4m, ProductNutritionCategoryId = 7 },

                // Горіхи та насіння (8)
                new() { Name = "Мигдаль", CaloriesPer100Grams = 579, ProteinPer100Grams = 21, FatPer100Grams = 50, CarbohydratesPer100Grams = 22, ProductNutritionCategoryId = 8 },
                new() { Name = "Арахіс", CaloriesPer100Grams = 567, ProteinPer100Grams = 26, FatPer100Grams = 49, CarbohydratesPer100Grams = 16, ProductNutritionCategoryId = 8 },
                new() { Name = "Насіння соняшника", CaloriesPer100Grams = 584, ProteinPer100Grams = 20, FatPer100Grams = 52, CarbohydratesPer100Grams = 20, ProductNutritionCategoryId = 8 },

                // Фастфуд та снеки (9)
                new() { Name = "Картопля фрі", CaloriesPer100Grams = 312, ProteinPer100Grams = 3.4m, FatPer100Grams = 15, CarbohydratesPer100Grams = 41, ProductNutritionCategoryId = 9 },
                new() { Name = "Бургер з яловичиною", CaloriesPer100Grams = 295, ProteinPer100Grams = 17, FatPer100Grams = 14, CarbohydratesPer100Grams = 26, ProductNutritionCategoryId = 9 },
                new() { Name = "Чіпси", CaloriesPer100Grams = 536, ProteinPer100Grams = 7, FatPer100Grams = 35, CarbohydratesPer100Grams = 50, ProductNutritionCategoryId = 9 },

                // Десерти (10)
                new() { Name = "Шоколад молочний", CaloriesPer100Grams = 535, ProteinPer100Grams = 7, FatPer100Grams = 30, CarbohydratesPer100Grams = 59, ProductNutritionCategoryId = 10 },
                new() { Name = "Тістечко з кремом", CaloriesPer100Grams = 350, ProteinPer100Grams = 4.5m, FatPer100Grams = 20, CarbohydratesPer100Grams = 38, ProductNutritionCategoryId = 10 },
                new() { Name = "Морозиво ванільне", CaloriesPer100Grams = 207, ProteinPer100Grams = 3.5m, FatPer100Grams = 11, CarbohydratesPer100Grams = 24, ProductNutritionCategoryId = 10 }

            };

            await context.ProductNutritions.AddRangeAsync(products);
        }
        
        await context.SaveChangesAsync();
    }
}