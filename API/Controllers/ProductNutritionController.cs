using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO.ProductNutrition;
using NutriTrack.Entities;
using NutriTrack.Extensions;
using NutriTrack.RequestHelpers;
using NutriTrack.Services;
using NutriTrack.Tests;

namespace NutriTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductNutritionController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IImageService _imageService; 

    public ProductNutritionController(ApplicationDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetProductNutrition(int id)
    {
        var productNutrition = await _context.ProductNutritions
            .Where(n => n.Id == id)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                CaloriesPer100Grams = p.CaloriesPer100Grams,
                ProteinPer100Grams = p.ProteinPer100Grams,
                FatPer100Grams = p.FatPer100Grams,
                CarbohydratesPer100Grams = p.CarbohydratesPer100Grams,
                CategoryName = p.ProductNutritionCategory.Name,
                ImageId = p.ImageUrl
            })
            .FirstOrDefaultAsync();

        if (productNutrition == null) return NotFound();

        return productNutrition;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedList<ProductResponse>>> GetProducts(
        [FromQuery] ProductNutritionParams productNutritionParams)
    {
        var query = _context.ProductNutritions
            .Include(p => p.ProductNutritionCategory)
            .Where(p => p.IsDeleted == false)
            .Sort(productNutritionParams.OrderBy)
            .Search(productNutritionParams.SearchTerm)
            .FilterByCategories(productNutritionParams.ProductNutritionCategory)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                CaloriesPer100Grams = p.CaloriesPer100Grams,
                ProteinPer100Grams = p.ProteinPer100Grams,
                CarbohydratesPer100Grams = p.CarbohydratesPer100Grams,
                FatPer100Grams = p.FatPer100Grams,
                CategoryName = p.ProductNutritionCategory.Name,
                ImageId = p.ImageUrl!
            })
            .AsQueryable();

        var products =
            await PagedList<ProductResponse>.ToPagedList(query, productNutritionParams.PageNumber,
                productNutritionParams.PageSize);

        Response.AddPaginationHeader(products.MetaData);

        return products;
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<ProductResponse>>> GetAllProductsAsync()
    {
        var products = await _context.ProductNutritions
            .Include(p => p.ProductNutritionCategory)
            .Where(p => p.IsDeleted == false)
            .OrderBy(p => p.Name)
            .ToListAsync();

        var productsDto = products.Select(product => new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            CaloriesPer100Grams = product.CaloriesPer100Grams,
            ProteinPer100Grams = product.ProteinPer100Grams,
            FatPer100Grams = product.FatPer100Grams,
            CarbohydratesPer100Grams = product.CarbohydratesPer100Grams,
            ImageId = product.ImageUrl!,
            CategoryName = product.ProductNutritionCategory.Name,
        }).ToList();

        return Ok(productsDto);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<ActionResult<ProductResponse>> CreateProductNutrition([FromForm]CreateProductRequest request)
    {
        var newProduct = new ProductNutrition
        {
            Name = request.Name,
            CaloriesPer100Grams = request.CaloriesPer100Grams,
            ProteinPer100Grams = request.ProteinPer100Grams,
            FatPer100Grams = request.FatPer100Grams,
            CarbohydratesPer100Grams = request.CarbohydratesPer100Grams,
        };
        
        var category = await _context.ProductNutritionCategories
            .Where(c => c.Name == request.CategoryName)
            .SingleOrDefaultAsync();
        
        if (category is null) return NotFound("Category not found");
        
        newProduct.ProductNutritionCategory = category;

        if (request.File is not null)
        {
            var imageUrl = await _imageService.UploadImageAsync(request.File);
            newProduct.ImageUrl = imageUrl;
        }
        
        await _context.ProductNutritions.AddAsync(newProduct);

        var result = await _context.SaveChangesAsync() > 0;
        
        var productResponse = new ProductResponse
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            CaloriesPer100Grams = newProduct.CaloriesPer100Grams,
            ProteinPer100Grams = newProduct.ProteinPer100Grams,
            CarbohydratesPer100Grams = newProduct.CarbohydratesPer100Grams,
            FatPer100Grams = newProduct.FatPer100Grams,
            CategoryName = newProduct.ProductNutritionCategory.Name,
            ImageId = newProduct.ImageUrl!
        };
        
        if (result) return Ok(productResponse);

        return BadRequest("Виникла проблема з додаванням продукту!");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<ActionResult> UpdateProductNutrition([FromForm]UpdateProductRequest request)
    {
        var updatedProduct = await _context.ProductNutritions
            .SingleOrDefaultAsync(p => p.Id == request.Id);

        if (updatedProduct is null)
            return NotFound("Продукт було не знайдено!");
        
        updatedProduct.Name = request.Name;
        updatedProduct.CaloriesPer100Grams = request.CaloriesPer100Grams;
        updatedProduct.ProteinPer100Grams = request.ProteinPer100Grams;
        updatedProduct.FatPer100Grams = request.FatPer100Grams;
        updatedProduct.CarbohydratesPer100Grams = request.CarbohydratesPer100Grams;

        var category = await _context.ProductNutritionCategories
            .SingleOrDefaultAsync(c => c.Name == request.CategoryName);

        if (category == null)
            return NotFound("Вказана катерогія не була знайдена");
    
        updatedProduct.ProductNutritionCategory = category;

        if (request.File != null)
        {
            var imageUrl = await _imageService.UploadImageAsync(request.File);
            
            if (!string.IsNullOrEmpty(updatedProduct.ImageUrl)) await _imageService.DeleteImageAsync(updatedProduct.ImageUrl);
            
            updatedProduct.ImageUrl = imageUrl;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _context.ProductNutritions.FindAsync(id);

        if (product == null) return NotFound();

        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            await _imageService.DeleteImageAsync(product.ImageUrl);
        }

        product.IsDeleted = true;

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Виникла проблема при видаленні продукту!");
    }
    
    [HttpGet("categories")]
    public async Task<ActionResult<List<ProductCategoryResponse>>> GetProductNutritionCategories()
    {
        var categories = await _context.ProductNutritionCategories
            .Where(c => !c.IsDeleted)
            .Select(c => new ProductCategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                IsDeleteable = c.ProductNutritions.All(p => p.IsDeleted)
            })
            .ToListAsync();

        return categories;
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("category/add")]
    public async Task<ActionResult> AddNewProductNutritionCategory([FromBody] CreateProductCategoryRequest request)
    {
        var category = await _context.ProductNutritionCategories
            .FirstOrDefaultAsync(c => c.Name == request.CategoryName);
        
        if (category != null)
        {
            if (!category.IsDeleted)
            {
                throw new ValidationException($"Категорія з назвою '{request.CategoryName}' вже існує в базі даних!");
            }

            category.IsDeleted = false;
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        var newCategory = new ProductNutritionCategory
        {
            Name = request.CategoryName
        };
        
        await _context.ProductNutritionCategories.AddAsync(newCategory);
        await _context.SaveChangesAsync();
        
        return Created();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("category/{id}")]
    public async Task<ActionResult> DeleteProductNutritionCategory([Required] int id)
    {
        var category = await _context.ProductNutritionCategories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null || category.IsDeleted)
        {
            return NotFound($"Категорія під ідентифікатором {id} не існує у базі даних! ");
        }
        
        category.IsDeleted = true;
        
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
}