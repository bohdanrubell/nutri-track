using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.DTO.ProductNutrition;
using NutriTrack.Entities;
using NutriTrack.Extensions;
using NutriTrack.RequestHelpers;
using NutriTrack.Services;

namespace NutriTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductNutritionController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ImageService _imageService;
    private readonly IMapper _mapper;

    public ProductNutritionController(ApplicationDbContext context, IMapper mapper, ImageService imageService)
    {
        _context = context;
        _mapper = mapper;
        _imageService = imageService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductNutritionResponse>> GetProductNutrition(int id)
    {
        var productNutrition = await _context.ProductNutritions
            .Where(n => n.Id == id)
            .Select(p => new ProductNutritionResponse
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

    [HttpGet("categories")]
    public async Task<ActionResult<List<ProductNutritionCategoryResponse>>> GetProductNutritionCategories()
    {
        return await _context.ProductNutritionCategories
            .Select(p => new ProductNutritionCategoryResponse
            {
                Name = p.Name
            })
            .ToListAsync();
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedList<ProductNutritionResponse>>> GetProducts(
        [FromQuery] ProductNutritionParams productNutritionParams)
    {
        var query = _context.ProductNutritions
            .Include(p => p.ProductNutritionCategory)
            .Sort(productNutritionParams.OrderBy)
            .Search(productNutritionParams.SearchTerm)
            .FilterByCategories(productNutritionParams.ProductNutritionCategory)
            .Select(p => new ProductNutritionResponse
            {
                Id = p.Id,
                Name = p.Name,
                CaloriesPer100Grams = p.CaloriesPer100Grams,
                ProteinPer100Grams = p.ProteinPer100Grams,
                CarbohydratesPer100Grams = p.CarbohydratesPer100Grams,
                FatPer100Grams = p.FatPer100Grams,
                CategoryName = p.ProductNutritionCategory.Name,
                ImageId = p.ImageUrl
            })
            .AsQueryable();

        var products =
            await PagedList<ProductNutritionResponse>.ToPagedList(query, productNutritionParams.PageNumber,
                productNutritionParams.PageSize);

        Response.AddPaginationHeader(products.MetaData);

        return products;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<ActionResult<ProductNutritionResponse>> CreateProductNutrition([FromForm]CreateProductNutritionRequest request)
    {
        var newProduct = _mapper.Map<ProductNutrition>(request);

        var category = await _context.ProductNutritionCategories
            .Where(c => c.Name == request.CategoryName)
            .SingleOrDefaultAsync();
        
        if (category is null) return NotFound("Category not found");
        
        newProduct.ProductNutritionCategory = category;
        
        string? imageUrl = null;

        if (request.File != null)
        {
            imageUrl = await _imageService.UploadImageAsync(request.File);
        }
        
        newProduct.ImageUrl = imageUrl;
        
        await _context.ProductNutritions.AddAsync(newProduct);

        var result = await _context.SaveChangesAsync() > 0;
        
        var productResponse = new ProductNutritionResponse
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            CaloriesPer100Grams = newProduct.CaloriesPer100Grams,
            ProteinPer100Grams = newProduct.ProteinPer100Grams,
            CarbohydratesPer100Grams = newProduct.CarbohydratesPer100Grams,
            FatPer100Grams = newProduct.FatPer100Grams,
            CategoryName = newProduct.ProductNutritionCategory.Name,
            ImageId = newProduct.ImageUrl
        };
        
        if (result) return Ok(productResponse);

        return BadRequest("Problem creating new product");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<ActionResult> UpdateProductNutrition(UpdateProductNutritionRequest request)
    {
        var updatedProduct = await _context.ProductNutritions
            .SingleOrDefaultAsync(p => p.Id == request.Id);

        if (updatedProduct == null) return NotFound();

        _mapper.Map(request, updatedProduct);

        var category = await _context.ProductNutritionCategories
            .Where(c => c.Name == request.CategoryName)
            .SingleOrDefaultAsync();
        
        if (category == null) return NotFound("Category not found");
        
        updatedProduct.ProductNutritionCategory = category;
        
        if (request.File != null)
        {
            var imageUrl = await _imageService.UploadImageAsync(request.File);
            updatedProduct.ImageUrl = imageUrl;
        }
        
        var result = await _context.SaveChangesAsync() > 0;

        if (result) return NoContent();

        return BadRequest("Problem updating product");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _context.ProductNutritions.FindAsync(id);

        if (product == null) return NotFound();
        
        _context.ProductNutritions.Remove(product);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Problem deleting the product");
    }
    
}