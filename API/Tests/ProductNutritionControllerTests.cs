using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Controllers;
using NutriTrack.Data;
using NutriTrack.DTO.ProductNutrition;
using NutriTrack.Entities;
using Xunit;

namespace NutriTrack.Tests
{
    public class ProductNutritionControllerTests
    {
        [Fact]
        public async Task CreateProductNutrition_WithValidData_ReturnsOkWithProductResponse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestProductNutritionDb")
                .Options;

            // Create DbContext with test data
            using var context = new ApplicationDbContext(options);
            
            // Add a test category
            var category = new ProductNutritionCategory
            {
                Id = 1,
                Name = "TestCategory"
            };
            
            await context.ProductNutritionCategories.AddAsync(category);
            await context.SaveChangesAsync();

            // Create a fake image service
            var imageService = new FakeImageService();

            // Create controller with test dependencies
            var controller = new ProductNutritionController(context, imageService);

            // Create test request
            var request = new CreateProductRequest
            {
                Name = "Test Product",
                CategoryName = "TestCategory",
                CaloriesPer100Grams = 100,
                ProteinPer100Grams = 10,
                FatPer100Grams = 5,
                CarbohydratesPer100Grams = 20,
                File = null // No file for simplicity
            };

            // Act
            var result = await controller.CreateProductNutrition(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var productResponse = Assert.IsType<ProductResponse>(okResult.Value);
            
            Assert.Equal(request.Name, productResponse.Name);
            Assert.Equal(request.CategoryName, productResponse.CategoryName);
            Assert.Equal(request.CaloriesPer100Grams, productResponse.CaloriesPer100Grams);
            Assert.Equal(request.ProteinPer100Grams, productResponse.ProteinPer100Grams);
            Assert.Equal(request.FatPer100Grams, productResponse.FatPer100Grams);
            Assert.Equal(request.CarbohydratesPer100Grams, productResponse.CarbohydratesPer100Grams);
            
            // Verify product was added to database
            var savedProduct = await context.ProductNutritions
                .Include(p => p.ProductNutritionCategory)
                .FirstOrDefaultAsync(p => p.Name == request.Name);
            
            Assert.NotNull(savedProduct);
            Assert.Equal(request.Name, savedProduct.Name);
            Assert.Equal(category.Id, savedProduct.ProductNutritionCategoryId);
        }
    }
} 