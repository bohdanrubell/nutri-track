using Microsoft.AspNetCore.Identity;
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
        await context.SaveChangesAsync();
    }
}
