using Microsoft.AspNetCore.Identity;
using NutriTrack.Entities;
using NutriTrack.Entity;

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
            new GoalType { Name = "WeightLoss", Percent = 85 },
            new GoalType { Name = "WeightMaintenance", Percent = 100 },
            new GoalType { Name = "WeightGain", Percent = 115 }
        };
        
        await context.GoalTypes.AddRangeAsync(goals);
        await context.SaveChangesAsync();
    }
}
