using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Entities;
using NutriTrack.Entity;

namespace NutriTrack.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
{
    public DbSet<WeightRecord> WeightRecords { get; set; }
    public DbSet<GoalType> GoalTypes { get; set; }
    public DbSet<GoalTypeLog> GoalTypeLogs { get; set; }
    public DbSet<ActivityLevel> ActivityLevels { get; set; }
    public DbSet<ActivityLevelLog> ActivityLevelLogs { get; set; }
    public DbSet<ProductNutritionCategory> ProductNutritionCategories { get; set; }
    public DbSet<ProductNutrition> ProductNutritions { get; set; }
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
        
        builder.Entity<ProductNutritionCategory>()
            .HasMany(c => c.ProductNutritions)
            .WithOne(p => p.ProductNutritionCategory)
            .HasForeignKey(p => p.ProductNutritionCategoryId);
        
        //Activity and Goal types
        
        // Налаштування зв'язку "один до багатьох" між User та ActivityLevelLog
        builder.Entity<User>()
            .HasMany(u => u.ActivityLevelLogs)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);
        
        // Налаштування зв'язку "один до багатьох" між User та GoalTypeLog
        builder.Entity<User>()
            .HasMany(u => u.GoalTypeLogs)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);
        
        builder.Entity<GoalType>()
            .HasMany(g => g.Logs)
            .WithOne(l => l.Goal)
            .HasForeignKey(l => l.GoalTypeId);
        
        builder.Entity<ActivityLevel>()
            .HasMany(g => g.Logs)
            .WithOne(l => l.ActivityLevel)
            .HasForeignKey(l => l.ActivityId);
        //Activity and Goal types
    }
}