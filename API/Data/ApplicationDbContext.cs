using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Entity;

namespace NutriTrack.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
{
    public DbSet<WeightRecord> WeightRecords { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .Property(u => u.UserGender)
            .HasConversion<string>();
        
        builder.Entity<WeightRecord>()
            .HasOne(wr => wr.User)
            .WithMany(u => u.WeightRecords)
            .HasForeignKey(wr => wr.UserId);
        
        builder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "User", NormalizedName = "USER" }
            );
    }
}