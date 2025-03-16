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
    public DbSet<Diary> Diaries { get; set; }
    public DbSet<Record> Records { get; set; }
    public DbSet<ProductRecord> ProductRecords { get; set; }
    
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
                new Role { Id = 1, Name = "User", NormalizedName = "USER" },
                new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
            );
        
        builder.Entity<ProductNutritionCategory>()
            .HasMany(c => c.ProductNutritions)
            .WithOne(p => p.ProductNutritionCategory)
            .HasForeignKey(p => p.ProductNutritionCategoryId);
        
        builder.Entity<User>()
            .HasOne(u => u.Diary)
            .WithOne(d => d.User)
            .HasForeignKey<Diary>(d => d.UserId);
        
        builder.Entity<ProductNutrition>()
            .HasMany(pn => pn.ProductRecords)
            .WithOne(pr => pr.ProductNutrition)
            .HasForeignKey(pr => pr.ProductNutritionId);
        
        builder.Entity<Record>()
            .HasMany(r => r.ProductRecords)
            .WithOne(pr => pr.Record)
            .HasForeignKey(pr => pr.RecordId);
        
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