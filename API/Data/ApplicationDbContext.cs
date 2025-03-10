using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Entity;

namespace NutriTrack.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "User", NormalizedName = "USER" }
            );
    }
}