using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NutriTrack.Data;
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
        var productNutrition = await _context.ProductNutritions.FindAsync(id);

        if (productNutrition == null) return NotFound();

        return productNutrition;
    }
}