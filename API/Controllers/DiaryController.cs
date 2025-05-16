using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.DTO.ProductRecord;
using NutriTrack.DTO.Statistics;
using NutriTrack.DTO.User;
using NutriTrack.Entities;
using NutriTrack.Entities.Enums;
using NutriTrack.Exceptions;
using NutriTrack.Services;

namespace NutriTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiaryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;

    public DiaryController(ApplicationDbContext context,
        UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpGet("getRecordByDate/{date}")]
    public async Task<ActionResult<DairyRecordResponse>> GetRecordByDate(DateTime date)
    {
        var user = await _userService.GetUserAsync();

        var record = await _context.Records
            .Include(r => r.ProductRecords)
            .ThenInclude(pr => pr.ProductNutrition)
            .FirstOrDefaultAsync(r => r.Diary.User.Id == user.Id && r.Date.Date == date.Date);

        if (record is null)
            return NotFound();

        var recordResponse = new DairyRecordResponse
        {
            DailyNutritions = new DailyNutritionsResponse
            {
                DailyCalories = record.DailyCalories,
                DailyProtein = record.DailyProtein,
                DailyFat = record.DailyFat,
                DailyCarbohydrates = record.DailyCarbohydrates
            },
            ProductRecords = record.ProductRecords.Select(pr => new ProductRecordResponse
            {
                Id = pr.Id,
                Name = pr.ProductNutrition.Name,
                Grams = pr.Grams,
                Calories = (int)(pr.ProductNutrition.CaloriesPer100Grams * pr.Grams / 100),
                Protein = Math.Round(pr.ProductNutrition.ProteinPer100Grams * pr.Grams / 100, 1),
                Fat = Math.Round(pr.ProductNutrition.FatPer100Grams * pr.Grams / 100,1),
                Carbohydrates = Math.Round(pr.ProductNutrition.CarbohydratesPer100Grams * pr.Grams / 100, 1)
            }).ToList()
        };

        return Ok(recordResponse);
    }

    [HttpPost("addNewProductRecord")]
    public async Task<ActionResult> AddProductToRecord(ProductRecordRequest productRecordRequest)
    {
        if (productRecordRequest == null || productRecordRequest.ProductNutritionId <= 0)
        {
            return BadRequest("Invalid product record request.");
        }
        
        var user = await _userService.GetUserAsync();
        var currentDate = productRecordRequest.Date?.Date ?? DateTime.Now;

        var record = await GetOrCreateRecordAsync(user.Id, currentDate);

        var productNutrition = await _context.ProductNutritions
            .FirstOrDefaultAsync(p => p.Id == productRecordRequest.ProductNutritionId);

        if (productNutrition == null)
        {
            return NotFound("Продукт не знайдено");
        }

        var isProductAlreadyAdded = await _context.ProductRecords
            .AnyAsync(p => p.RecordId == record.Id && p.ProductNutritionId == productRecordRequest.ProductNutritionId);

       if (isProductAlreadyAdded)
        {
            var existingProductRecord = await _context.ProductRecords
                .FirstOrDefaultAsync(p => p.RecordId == record.Id && p.ProductNutritionId == productRecordRequest.ProductNutritionId);
        
            if (existingProductRecord != null)
            {
                existingProductRecord.Grams += productRecordRequest.ConsumedGrams;
                await _context.SaveChangesAsync();
                return Ok(existingProductRecord.Id);
            }
        }

        var newProductRecord = ProductRecord.Create(record, productNutrition, productRecordRequest.ConsumedGrams);

        await _context.ProductRecords.AddAsync(newProductRecord);
        await _context.SaveChangesAsync();

        return Ok(newProductRecord.Id);
    }
    
    private async Task<Record> GetOrCreateRecordAsync(Guid userId, DateTime currentDate)
    {
        var diary = await _context.Diaries
            .Include(d => d.Records)
            .FirstOrDefaultAsync(d => d.UserId == userId);
    
        if (diary == null)
        {
            throw new InvalidOperationException("Diary not found for the user.");
        }
    
        var record = diary.Records.FirstOrDefault(r => r.Date == currentDate);
        if (record != null)
        {
            return record;
        }
    
        var userCurrentGoalType = await _userService.GetLastUsersGoalTypeLog(userId);
        var userCurrentActivityLevel = await _userService.GetLastUserActivityLevelLog(userId);
        var userWeightRecord = await _context.WeightRecords
            .Where(w => w.User.Id == userId)
            .OrderByDescending(w => w.DateOfRecordCreated)
            .Select(w => new { w.Weight })
            .FirstOrDefaultAsync();
    
        if (userWeightRecord == null)
        {
            throw new InvalidOperationException("User weight record not found.");
        }
    
        var user = await _userService.GetUserAsync();
        var age = DateTime.Now.Year - user.DateOfBirth.Year;
        if (DateTime.Now.DayOfYear < user.DateOfBirth.DayOfYear) age--;
    
        var calculator = new CaloriesCalc(user.UserGender, age, user.Height, userWeightRecord.Weight,
            userCurrentActivityLevel.ActivityLevel, userCurrentGoalType.Goal);
    
        record = Record.Create(currentDate,
            calculator.CalculateDailyCalories(), calculator.CalculateDailyProtein(),
            calculator.CalculateDailyFat(), calculator.CalculateDailyCarbohydrates(),
            diary, userCurrentActivityLevel, userCurrentGoalType);
    
        await _context.Records.AddAsync(record);
        await _context.SaveChangesAsync();
    
        return record;
    }

    [HttpPut("updateProductRecord")]
    public async Task<ActionResult> UpdateProductRecord([FromBody] ProductRecordUpdateRequest productRecordRequest)
    {
        var user = await _userService.GetUserAsync();

        var product = await _context.ProductRecords
            .Include(p => p.Record)
            .ThenInclude(p => p.Diary)
            .FirstOrDefaultAsync(p =>
                p.Id == productRecordRequest.ProductRecordId && p.Record.Diary.User.Id == user.Id);

        if (product is null) return NotFound("Продукт не знайдено");

        product.Grams = productRecordRequest.ConsumedGrams;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("deleteProductRecord/{id}")]
    public async Task<ActionResult> DeleteProductRecord(int id)
    {
        var user = await _userService.GetUserAsync();

        var product = await _context.ProductRecords
            .Include(p => p.Record)
            .ThenInclude(p => p.Diary)
            .FirstOrDefaultAsync(p => p.Id == id && p.Record.Diary.User.Id == user.Id);

        if (product is null) return NotFound("Продукт не знайдено");

        _context.ProductRecords.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("getStatisticsByPeriod/{period}")]
    public async Task<ActionResult<List<PeriodStatisticsResponse>>> GetStatisticsByPeriodAsync(string period)
    {
            var user = await _userService.GetUserAsync();

            var today = DateTime.Today;
            DateTime startDate, endDate;

            switch (period.ToLower())
            {
                case "last3days":
                    startDate = today.AddDays(-2);
                    endDate = today;
                    break;

                case "currentweek":
                    var dow = (int)today.DayOfWeek;
                    var currentWeekStart = today.AddDays(-(dow == 0 ? 6 : dow - 1));
                    startDate = currentWeekStart;
                    endDate = today;
                    break;

                case "previousweek":
                    var d = (int)today.DayOfWeek;
                    var thisWeekStart = today.AddDays(-(d == 0 ? 6 : d - 1));
                    startDate = thisWeekStart.AddDays(-7);
                    endDate = thisWeekStart.AddDays(-1);
                    break;

                default:
                    return BadRequest("Invalid period");
            }

            var records = await _context.Records
                .Include(r => r.ProductRecords)
                .ThenInclude(pr => pr.ProductNutrition)
                .Where(r => r.Date >= startDate && r.Date <= endDate && r.Diary.User.Id == user.Id)
                .ToListAsync();

            var dailyStats = records
                .GroupBy(r => r.Date.Date)
                .Select(g =>
                {
                    var totalCalories = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => pr.Grams / 100 * pr.ProductNutrition.CaloriesPer100Grams);
                    var totalProteins = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => pr.Grams / 100 * pr.ProductNutrition.ProteinPer100Grams);
                    var totalFats = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => pr.Grams / 100 * pr.ProductNutrition.FatPer100Grams);
                    var totalCarbs = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => pr.Grams / 100 * pr.ProductNutrition.CarbohydratesPer100Grams);

                    var calories = (int)Math.Round(totalCalories);
                    var proteins = Math.Round(totalProteins);
                    var fats = Math.Round(totalFats);
                    var carbs = Math.Round(totalCarbs);

                    var first = g.First();

                    var status = GetNormStatus(
                        calories, first.DailyCalories,
                        proteins, first.DailyProtein,
                        fats, first.DailyFat,
                        carbs, first.DailyCarbohydrates
                    );

                    return new PeriodStatisticsResponse
                    {
                        Date = g.Key.ToString("yyyy-MM-dd"),
                        ConsumedCalories = calories,
                        ConsumedProteins = proteins,
                        ConsumedFats = fats,
                        ConsumedCarbohydrates = carbs,
                        Status = status.Status.ToString(),
                        ExceededNutrients = status.ExceededNutrients
                    };

                })
                .OrderBy(d => d.Date)
                .ToList();


            return Ok(dailyStats);
    }

    private static NormStatusResult GetNormStatus(
        int calories, int normCalories,
        decimal proteins, decimal normProteins,
        decimal fats, decimal normFats,
        decimal carbs, decimal normCarbohydrates)
    {
        var checks = new List<(string Name, bool IsExceeded, bool IsReached)>
        {
            Evaluate("Калорії", calories, normCalories),
            Evaluate("Білки", proteins, normProteins),
            Evaluate("Жири", fats, normFats),
            Evaluate("Вуглеводи", carbs, normCarbohydrates)
        };

        var exceeded = checks.Where(c => c.IsExceeded).Select(c => c.Name).ToList();

        var status = exceeded.Any()
            ? NormStatus.Exceeded
            : checks.All(c => c.IsReached)
                ? NormStatus.Reached
                : NormStatus.NotReached;

        return new NormStatusResult
        {
            Status = status,
            ExceededNutrients = exceeded
        };
    }


    private static (string Name, bool IsExceeded, bool IsReached) Evaluate(string name, int value, int norm)
    {
        bool exceeded = value > norm * 1.15;
        bool reached = value >= norm;
        return (name, exceeded, reached);
    }

    private static (string Name, bool IsExceeded, bool IsReached) Evaluate(string name, decimal value, decimal norm)
    {
        bool exceeded = value > norm * 1.15m;
        bool reached = value >= norm;
        return (name, exceeded, reached);
    }


}