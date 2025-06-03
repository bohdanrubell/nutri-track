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
        
        [Fact]
        public async Task GetProductNutrition_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestGetProductNotFoundDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var imageService = new FakeImageService();
            var controller = new ProductNutritionController(context, imageService);

            // Act
            var result = await controller.GetProductNutrition(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public async Task DeleteProduct_WithValidId_MarksProductAsDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDeleteProductDb")
                .Options;

            using var context = new ApplicationDbContext(options);
    
            // Add test category and product
            var category = new ProductNutritionCategory { Id = 1, Name = "TestCategory" };
            await context.ProductNutritionCategories.AddAsync(category);
    
            var product = new ProductNutrition
            {
                Id = 1,
                Name = "Product to Delete",
                CaloriesPer100Grams = 100,
                ProteinPer100Grams = 10,
                FatPer100Grams = 5,
                CarbohydratesPer100Grams = 20,
                ProductNutritionCategoryId = 1,
                IsDeleted = false
            };
    
            await context.ProductNutritions.AddAsync(product);
            await context.SaveChangesAsync();

            var imageService = new FakeImageService();
            var controller = new ProductNutritionController(context, imageService);

            // Act
            var result = await controller.DeleteProduct(1);

            // Assert
            Assert.IsType<OkResult>(result);
    
            // Verify product is marked as deleted
            var deletedProduct = await context.ProductNutritions.FindAsync(1);
            Assert.NotNull(deletedProduct);
            Assert.True(deletedProduct.IsDeleted);
        }
        
        [Fact]
        public async Task UpdateProductNutrition_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestUpdateProductDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            
            // Add test categories
            var category1 = new ProductNutritionCategory { Id = 1, Name = "Fruits" };
            var category2 = new ProductNutritionCategory { Id = 2, Name = "Vegetables" };
            await context.ProductNutritionCategories.AddRangeAsync(category1, category2);
            
            // Add a test product
            var product = new ProductNutrition
            {
                Id = 1,
                Name = "Original Apple",
                CaloriesPer100Grams = 52,
                ProteinPer100Grams = 0.3m,
                FatPer100Grams = 0.2m,
                CarbohydratesPer100Grams = 14,
                ProductNutritionCategoryId = 1
            };
            
            await context.ProductNutritions.AddAsync(product);
            await context.SaveChangesAsync();

            var imageService = new FakeImageService();
            var controller = new ProductNutritionController(context, imageService);

            var updateRequest = new UpdateProductRequest
            {
                Id = 1,
                Name = "Updated Green Apple",
                CategoryName = "Vegetables", // Change category
                CaloriesPer100Grams = 55,
                ProteinPer100Grams = 0.5m,
                FatPer100Grams = 0.1m,
                CarbohydratesPer100Grams = 15,
                File = null
            };

            // Act
            var result = await controller.UpdateProductNutrition(updateRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify the product was updated in the database
            var updatedProduct = await context.ProductNutritions
                .Include(p => p.ProductNutritionCategory)
                .FirstOrDefaultAsync(p => p.Id == 1);

            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Green Apple", updatedProduct.Name);
            Assert.Equal("Vegetables", updatedProduct.ProductNutritionCategory.Name);
            Assert.Equal(55, updatedProduct.CaloriesPer100Grams);
            Assert.Equal(0.5m, updatedProduct.ProteinPer100Grams);
            Assert.Equal(0.1m, updatedProduct.FatPer100Grams);
            Assert.Equal(15, updatedProduct.CarbohydratesPer100Grams);
        }
        
        [Fact]
        public async Task GetProductNutritionCategories_ReturnsOnlyNonDeletedCategories()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestGetCategoriesDb")
                .Options;

            using var context = new ApplicationDbContext(options);
    
            // Add test categories - some deleted, some not
            var categories = new[]
            {
                new ProductNutritionCategory { Id = 1, Name = "Fruits", IsDeleted = false },
                new ProductNutritionCategory { Id = 2, Name = "Vegetables", IsDeleted = false },
                new ProductNutritionCategory { Id = 3, Name = "Deleted Category", IsDeleted = true },
                new ProductNutritionCategory { Id = 4, Name = "Meat", IsDeleted = false }
            };
    
            await context.ProductNutritionCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var imageService = new FakeImageService();
            var controller = new ProductNutritionController(context, imageService);

            // Act
            var result = await controller.GetProductNutritionCategories();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<ProductCategoryResponse>>>(result);
            Assert.NotNull(actionResult.Value);
    
            var categoriesList = actionResult.Value;
    
            // Should only return non-deleted categories
            Assert.Equal(3, categoriesList.Count);
            Assert.Contains(categoriesList, c => c.Name == "Fruits");
            Assert.Contains(categoriesList, c => c.Name == "Vegetables");
            Assert.Contains(categoriesList, c => c.Name == "Meat");
            Assert.DoesNotContain(categoriesList, c => c.Name == "Deleted Category");
    
            // Verify all returned categories have proper structure
            foreach (var category in categoriesList)
            {
                Assert.True(category.Id > 0);
                Assert.False(string.IsNullOrEmpty(category.Name));
                Assert.NotNull(category.IsDeleteable); // This property should be set
            }
        }
    }
} 