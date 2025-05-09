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
            new ProductNutritionCategory { Id = 4, Name = "М'ясо" }
        );

        builder.Entity<ProductNutrition>().HasData(
            // Фрукти
            new ProductNutrition
            {
                Id = 1, Name = "Яблуко", CaloriesPer100Grams = 52, ProteinPer100Grams = 0, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 14, ProductNutritionCategoryId = 1
            },
            new ProductNutrition
            {
                Id = 2, Name = "Банан", CaloriesPer100Grams = 96, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 23, ProductNutritionCategoryId = 1
            },
            new ProductNutrition
            {
                Id = 3, Name = "Апельсин", CaloriesPer100Grams = 47, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 12, ProductNutritionCategoryId = 1
            },
            new ProductNutrition
            {
                Id = 4, Name = "Полуниця", CaloriesPer100Grams = 32, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 8, ProductNutritionCategoryId = 1
            },
            new ProductNutrition
            {
                Id = 5, Name = "Виноград", CaloriesPer100Grams = 69, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 18, ProductNutritionCategoryId = 1
            },

            // Овочі
            new ProductNutrition
            {
                Id = 6, Name = "Морковка", CaloriesPer100Grams = 41, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 10, ProductNutritionCategoryId = 2
            },
            new ProductNutrition
            {
                Id = 7, Name = "Броколі", CaloriesPer100Grams = 55, ProteinPer100Grams = 4, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 11, ProductNutritionCategoryId = 2
            },
            new ProductNutrition
            {
                Id = 8, Name = "Помідор", CaloriesPer100Grams = 18, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },
            new ProductNutrition
            {
                Id = 9, Name = "Шпинат", CaloriesPer100Grams = 23, ProteinPer100Grams = 3, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },
            new ProductNutrition
            {
                Id = 10, Name = "Огірок", CaloriesPer100Grams = 16, ProteinPer100Grams = 1, FatPer100Grams = 0,
                CarbohydratesPer100Grams = 4, ProductNutritionCategoryId = 2
            },

            // Молочні продукти
            new ProductNutrition
            {
                Id = 11, Name = "Молоко", CaloriesPer100Grams = 42, ProteinPer100Grams = 3, FatPer100Grams = 1,
                CarbohydratesPer100Grams = 5, ProductNutritionCategoryId = 3
            },
            new ProductNutrition
            {
                Id = 12, Name = "Йогурт", CaloriesPer100Grams = 59, ProteinPer100Grams = 3, FatPer100Grams = 2,
                CarbohydratesPer100Grams = 7, ProductNutritionCategoryId = 3
            },
            new ProductNutrition
            {
                Id = 13, Name = "Сир", CaloriesPer100Grams = 402, ProteinPer100Grams = 25, FatPer100Grams = 33,
                CarbohydratesPer100Grams = 1, ProductNutritionCategoryId = 3
            },
            new ProductNutrition
            {
                Id = 14, Name = "Масло", CaloriesPer100Grams = 717, ProteinPer100Grams = 1, FatPer100Grams = 81,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 3
            },
            new ProductNutrition
            {
                Id = 15, Name = "Сир кисломолочий", CaloriesPer100Grams = 98, ProteinPer100Grams = 11,
                FatPer100Grams = 4, CarbohydratesPer100Grams = 3, ProductNutritionCategoryId = 3
            },

            // М'ясо
            new ProductNutrition
            {
                Id = 16, Name = "Курине філе", CaloriesPer100Grams = 165, ProteinPer100Grams = 31, FatPer100Grams = 4,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new ProductNutrition
            {
                Id = 17, Name = "Яловичина", CaloriesPer100Grams = 250, ProteinPer100Grams = 26, FatPer100Grams = 15,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new ProductNutrition
            {
                Id = 18, Name = "Свинячий стейк", CaloriesPer100Grams = 242, ProteinPer100Grams = 27,
                FatPer100Grams = 14, CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new ProductNutrition
            {
                Id = 19, Name = "Індичка", CaloriesPer100Grams = 189, ProteinPer100Grams = 29, FatPer100Grams = 7,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            },
            new ProductNutrition
            {
                Id = 20, Name = "Каре ягня", CaloriesPer100Grams = 294, ProteinPer100Grams = 25, FatPer100Grams = 21,
                CarbohydratesPer100Grams = 0, ProductNutritionCategoryId = 4
            }
        );
    }
}