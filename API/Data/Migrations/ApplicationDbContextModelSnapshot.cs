﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NutriTrack.Data;

#nullable disable

namespace NutriTrack.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("NutriTrack.Entities.ActivityLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ratio")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ActivityLevels");
                });

            modelBuilder.Entity("NutriTrack.Entities.ActivityLevelLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActivityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("ActivityLevelLogs");
                });

            modelBuilder.Entity("NutriTrack.Entities.Diary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateDiaryCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Diaries");
                });

            modelBuilder.Entity("NutriTrack.Entities.GoalType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Percent")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GoalTypes");
                });

            modelBuilder.Entity("NutriTrack.Entities.GoalTypeLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("GoalTypeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoalTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("GoalTypeLogs");
                });

            modelBuilder.Entity("NutriTrack.Entities.ProductNutrition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CaloriesPer100Grams")
                        .HasColumnType("int");

                    b.Property<double>("CarbohydratesPer100Grams")
                        .HasColumnType("float");

                    b.Property<double>("FatPer100Grams")
                        .HasColumnType("float");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductNutritionCategoryId")
                        .HasColumnType("int");

                    b.Property<double>("ProteinPer100Grams")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ProductNutritionCategoryId");

                    b.ToTable("ProductNutritions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CaloriesPer100Grams = 52,
                            CarbohydratesPer100Grams = 14.0,
                            FatPer100Grams = 0.0,
                            Name = "Яблуко",
                            ProductNutritionCategoryId = 1,
                            ProteinPer100Grams = 0.0
                        },
                        new
                        {
                            Id = 2,
                            CaloriesPer100Grams = 96,
                            CarbohydratesPer100Grams = 23.0,
                            FatPer100Grams = 0.0,
                            Name = "Банан",
                            ProductNutritionCategoryId = 1,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 3,
                            CaloriesPer100Grams = 47,
                            CarbohydratesPer100Grams = 12.0,
                            FatPer100Grams = 0.0,
                            Name = "Апельсин",
                            ProductNutritionCategoryId = 1,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 4,
                            CaloriesPer100Grams = 32,
                            CarbohydratesPer100Grams = 8.0,
                            FatPer100Grams = 0.0,
                            Name = "Полуниця",
                            ProductNutritionCategoryId = 1,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 5,
                            CaloriesPer100Grams = 69,
                            CarbohydratesPer100Grams = 18.0,
                            FatPer100Grams = 0.0,
                            Name = "Виноград",
                            ProductNutritionCategoryId = 1,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 6,
                            CaloriesPer100Grams = 41,
                            CarbohydratesPer100Grams = 10.0,
                            FatPer100Grams = 0.0,
                            Name = "Морковка",
                            ProductNutritionCategoryId = 2,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 7,
                            CaloriesPer100Grams = 55,
                            CarbohydratesPer100Grams = 11.0,
                            FatPer100Grams = 0.0,
                            Name = "Броколі",
                            ProductNutritionCategoryId = 2,
                            ProteinPer100Grams = 4.0
                        },
                        new
                        {
                            Id = 8,
                            CaloriesPer100Grams = 18,
                            CarbohydratesPer100Grams = 4.0,
                            FatPer100Grams = 0.0,
                            Name = "Помідор",
                            ProductNutritionCategoryId = 2,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 9,
                            CaloriesPer100Grams = 23,
                            CarbohydratesPer100Grams = 4.0,
                            FatPer100Grams = 0.0,
                            Name = "Шпинат",
                            ProductNutritionCategoryId = 2,
                            ProteinPer100Grams = 3.0
                        },
                        new
                        {
                            Id = 10,
                            CaloriesPer100Grams = 16,
                            CarbohydratesPer100Grams = 4.0,
                            FatPer100Grams = 0.0,
                            Name = "Огірок",
                            ProductNutritionCategoryId = 2,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 11,
                            CaloriesPer100Grams = 42,
                            CarbohydratesPer100Grams = 5.0,
                            FatPer100Grams = 1.0,
                            Name = "Молоко",
                            ProductNutritionCategoryId = 3,
                            ProteinPer100Grams = 3.0
                        },
                        new
                        {
                            Id = 12,
                            CaloriesPer100Grams = 59,
                            CarbohydratesPer100Grams = 7.0,
                            FatPer100Grams = 2.0,
                            Name = "Йогурт",
                            ProductNutritionCategoryId = 3,
                            ProteinPer100Grams = 3.0
                        },
                        new
                        {
                            Id = 13,
                            CaloriesPer100Grams = 402,
                            CarbohydratesPer100Grams = 1.0,
                            FatPer100Grams = 33.0,
                            Name = "Сир",
                            ProductNutritionCategoryId = 3,
                            ProteinPer100Grams = 25.0
                        },
                        new
                        {
                            Id = 14,
                            CaloriesPer100Grams = 717,
                            CarbohydratesPer100Grams = 0.0,
                            FatPer100Grams = 81.0,
                            Name = "Масло",
                            ProductNutritionCategoryId = 3,
                            ProteinPer100Grams = 1.0
                        },
                        new
                        {
                            Id = 15,
                            CaloriesPer100Grams = 98,
                            CarbohydratesPer100Grams = 3.0,
                            FatPer100Grams = 4.0,
                            Name = "Сир кисломолочий",
                            ProductNutritionCategoryId = 3,
                            ProteinPer100Grams = 11.0
                        },
                        new
                        {
                            Id = 16,
                            CaloriesPer100Grams = 165,
                            CarbohydratesPer100Grams = 0.0,
                            FatPer100Grams = 4.0,
                            Name = "Курине філе",
                            ProductNutritionCategoryId = 4,
                            ProteinPer100Grams = 31.0
                        },
                        new
                        {
                            Id = 17,
                            CaloriesPer100Grams = 250,
                            CarbohydratesPer100Grams = 0.0,
                            FatPer100Grams = 15.0,
                            Name = "Яловичина",
                            ProductNutritionCategoryId = 4,
                            ProteinPer100Grams = 26.0
                        },
                        new
                        {
                            Id = 18,
                            CaloriesPer100Grams = 242,
                            CarbohydratesPer100Grams = 0.0,
                            FatPer100Grams = 14.0,
                            Name = "Свинячий стейк",
                            ProductNutritionCategoryId = 4,
                            ProteinPer100Grams = 27.0
                        },
                        new
                        {
                            Id = 19,
                            CaloriesPer100Grams = 189,
                            CarbohydratesPer100Grams = 0.0,
                            FatPer100Grams = 7.0,
                            Name = "Індичка",
                            ProductNutritionCategoryId = 4,
                            ProteinPer100Grams = 29.0
                        },
                        new
                        {
                            Id = 20,
                            CaloriesPer100Grams = 294,
                            CarbohydratesPer100Grams = 0.0,
                            FatPer100Grams = 21.0,
                            Name = "Каре ягня",
                            ProductNutritionCategoryId = 4,
                            ProteinPer100Grams = 25.0
                        });
                });

            modelBuilder.Entity("NutriTrack.Entities.ProductNutritionCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductNutritionCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Фрукти"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Овочі"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Молочні продукти"
                        },
                        new
                        {
                            Id = 4,
                            Name = "М'ясо"
                        });
                });

            modelBuilder.Entity("NutriTrack.Entities.ProductRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Grams")
                        .HasColumnType("float");

                    b.Property<int>("ProductNutritionId")
                        .HasColumnType("int");

                    b.Property<int>("RecordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductNutritionId");

                    b.HasIndex("RecordId");

                    b.ToTable("ProductRecords");
                });

            modelBuilder.Entity("NutriTrack.Entities.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActivityLogId")
                        .HasColumnType("int");

                    b.Property<int>("DailyCalories")
                        .HasColumnType("int");

                    b.Property<double>("DailyCarbohydrates")
                        .HasColumnType("float");

                    b.Property<double>("DailyFat")
                        .HasColumnType("float");

                    b.Property<double>("DailyProtein")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DiaryId")
                        .HasColumnType("int");

                    b.Property<int>("GoalLogId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityLogId");

                    b.HasIndex("DiaryId");

                    b.HasIndex("GoalLogId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("NutriTrack.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserGender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("NutriTrack.Entity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("NutriTrack.Entity.WeightRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfRecordCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WeightRecords");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("NutriTrack.Entity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("NutriTrack.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("NutriTrack.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("NutriTrack.Entity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriTrack.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("NutriTrack.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NutriTrack.Entities.ActivityLevelLog", b =>
                {
                    b.HasOne("NutriTrack.Entities.ActivityLevel", "ActivityLevel")
                        .WithMany("Logs")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriTrack.Entities.User", "User")
                        .WithMany("ActivityLevelLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActivityLevel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutriTrack.Entities.Diary", b =>
                {
                    b.HasOne("NutriTrack.Entities.User", "User")
                        .WithOne("Diary")
                        .HasForeignKey("NutriTrack.Entities.Diary", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutriTrack.Entities.GoalTypeLog", b =>
                {
                    b.HasOne("NutriTrack.Entities.GoalType", "Goal")
                        .WithMany("Logs")
                        .HasForeignKey("GoalTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriTrack.Entities.User", "User")
                        .WithMany("GoalTypeLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Goal");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutriTrack.Entities.ProductNutrition", b =>
                {
                    b.HasOne("NutriTrack.Entities.ProductNutritionCategory", "ProductNutritionCategory")
                        .WithMany("ProductNutritions")
                        .HasForeignKey("ProductNutritionCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductNutritionCategory");
                });

            modelBuilder.Entity("NutriTrack.Entities.ProductRecord", b =>
                {
                    b.HasOne("NutriTrack.Entities.ProductNutrition", "ProductNutrition")
                        .WithMany("ProductRecords")
                        .HasForeignKey("ProductNutritionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriTrack.Entities.Record", "Record")
                        .WithMany("ProductRecords")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductNutrition");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("NutriTrack.Entities.Record", b =>
                {
                    b.HasOne("NutriTrack.Entities.ActivityLevelLog", "ActivityLog")
                        .WithMany()
                        .HasForeignKey("ActivityLogId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NutriTrack.Entities.Diary", "Diary")
                        .WithMany("Records")
                        .HasForeignKey("DiaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NutriTrack.Entities.GoalTypeLog", "GoalLog")
                        .WithMany()
                        .HasForeignKey("GoalLogId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ActivityLog");

                    b.Navigation("Diary");

                    b.Navigation("GoalLog");
                });

            modelBuilder.Entity("NutriTrack.Entity.WeightRecord", b =>
                {
                    b.HasOne("NutriTrack.Entities.User", "User")
                        .WithMany("WeightRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutriTrack.Entities.ActivityLevel", b =>
                {
                    b.Navigation("Logs");
                });

            modelBuilder.Entity("NutriTrack.Entities.Diary", b =>
                {
                    b.Navigation("Records");
                });

            modelBuilder.Entity("NutriTrack.Entities.GoalType", b =>
                {
                    b.Navigation("Logs");
                });

            modelBuilder.Entity("NutriTrack.Entities.ProductNutrition", b =>
                {
                    b.Navigation("ProductRecords");
                });

            modelBuilder.Entity("NutriTrack.Entities.ProductNutritionCategory", b =>
                {
                    b.Navigation("ProductNutritions");
                });

            modelBuilder.Entity("NutriTrack.Entities.Record", b =>
                {
                    b.Navigation("ProductRecords");
                });

            modelBuilder.Entity("NutriTrack.Entities.User", b =>
                {
                    b.Navigation("ActivityLevelLogs");

                    b.Navigation("Diary")
                        .IsRequired();

                    b.Navigation("GoalTypeLogs");

                    b.Navigation("WeightRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
