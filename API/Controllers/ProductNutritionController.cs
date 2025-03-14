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

    public ProductNutritionController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
    }

    [HttpGet("{id}", Name = "GetProductNutrition")]
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
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedList<ProductNutrition>>> GetProducts(
        [FromQuery] ProductNutritionParams productNutritionParams)
    {
        var query = _context.ProductNutritions
            .Sort(productNutritionParams.OrderBy)
            .Search(productNutritionParams.SearchTerm)
            .AsQueryable();

        var products =
            await PagedList<ProductNutrition>.ToPagedList(query, productNutritionParams.PageNumber,
                productNutritionParams.PageSize);

        Response.AddPaginationHeader(products.MetaData);

        return products;
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