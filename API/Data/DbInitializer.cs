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
        await context.SaveChangesAsync();
    }
}