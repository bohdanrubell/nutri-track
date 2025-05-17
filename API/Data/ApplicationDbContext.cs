using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Entities;
using NutriTrack.Entity;

namespace NutriTrack.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User, Role, Guid>(options)
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
        
        builder.Entity<IdentityUser>(b =>
        {
            b.ToTable("Users");
        });

        builder.Entity<IdentityRole>(b =>
        {
            b.ToTable("Roles");
        });

        builder.Entity<User>()
            .Property(u => u.UserGender)
            .HasConversion<string>();

        builder.Entity<ProductNutrition>(e =>
        {
            e.Property(p => p.CaloriesPer100Grams).HasColumnType("decimal(5,1)");
            e.Property(p => p.ProteinPer100Grams).HasColumnType("decimal(5,1)");
            e.Property(p => p.FatPer100Grams).HasColumnType("decimal(5,1)");
            e.Property(p => p.CarbohydratesPer100Grams).HasColumnType("decimal(5,1)");
        });

        builder.Entity<ProductRecord>(e =>
        {
            e.Property(p => p.Grams).HasColumnType("decimal(5,1)");
        });

        builder.Entity<Record>(e =>
        {
            e.Property(r => r.DailyProtein).HasColumnType("decimal(5,1)");
            e.Property(r => r.DailyFat).HasColumnType("decimal(5,1)");
            e.Property(r => r.DailyCarbohydrates).HasColumnType("decimal(5,1)");
        });

        
        builder.Entity<WeightRecord>()
            .HasOne(wr => wr.User)
            .WithMany(u => u.WeightRecords)
            .HasForeignKey(wr => wr.UserId);

        builder.Entity<Role>()
            .HasData(
                new Role { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" },
                new Role { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" }
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

        builder.Entity<Record>()
            .HasOne(r => r.Diary)
            .WithMany(d => d.Records)
            .HasForeignKey(r => r.DiaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Record>()
            .HasOne(r => r.ActivityLog)
            .WithMany()
            .HasForeignKey(r => r.ActivityLogId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Record>()
            .HasOne(r => r.GoalLog)
            .WithMany()
            .HasForeignKey(r => r.GoalLogId)
            .OnDelete(DeleteBehavior.Restrict);

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


        builder.Entity<ProductNutritionCategory>().HasData(
            new ProductNutritionCategory { Id = 1, Name = "Фрукти" },
            new ProductNutritionCategory { Id = 2, Name = "Овочі" },
            new ProductNutritionCategory { Id = 3, Name = "Молочні продукти" },
            new ProductNutritionCategory { Id = 4, Name = "М'ясо" },
            new ProductNutritionCategory { Id = 5, Name = "Зернові та хліб" },
            new ProductNutritionCategory { Id = 6, Name = "Крупи" },
            new ProductNutritionCategory { Id = 7, Name = "Напої" },
            new ProductNutritionCategory { Id = 8, Name = "Горіхи та насіння" },
            new ProductNutritionCategory { Id = 9, Name = "Фастфуд та снеки" },
            new ProductNutritionCategory { Id = 10, Name = "Десерти" }
        );
    }
}