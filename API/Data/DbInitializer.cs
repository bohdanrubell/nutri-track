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
        await context.SaveChangesAsync();
    }
}