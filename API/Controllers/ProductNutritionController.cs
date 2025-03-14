using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.Entities;

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
    
}