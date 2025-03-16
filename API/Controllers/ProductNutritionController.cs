using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.Entities;
using NutriTrack.Extensions;
using NutriTrack.RequestHelpers;

namespace NutriTrack.Controllers;

public class ProductNutritionController : BaseApiController
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductNutritionController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductNutrition>> GetProductNutrition(int id)
    {
        var productNutrition = await _context.ProductNutritions
            .Where(n => n.Id == id)
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
            .Select(p => new ProductNutritionResponse
            {
                Id = p.Id,
                Name = p.Name,
                Calories = p.CaloriesPer100Grams,
                Protein = p.ProteinPer100Grams,
                Carbohydrates = p.CarbohydratesPer100Grams,
                Fat = p.FatPer100Grams,
                Category = p.ProductNutritionCategory.Name,
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
    public async Task<ActionResult<ProductNutrition>> CreateProductNutrition(CreateProductNutritionRequest request)
    {
        var newProduct = _mapper.Map<ProductNutrition>(request);

        var category = await _context.ProductNutritionCategories
            .Where(c => c.Name == request.CategoryName)
            .SingleOrDefaultAsync();
        
        if (category is null) return NotFound("Category not found");
        
        newProduct.ProductNutritionCategory = category;
        
        await _context.ProductNutritions.AddAsync(newProduct);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Created();

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