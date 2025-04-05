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
        try
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
                    Grams = Math.Round(pr.Grams, 2),
                    Calories = (int)Math.Round(pr.ProductNutrition.CaloriesPer100Grams * pr.Grams / 100),
                    Protein = Math.Round(pr.ProductNutrition.ProteinPer100Grams * pr.Grams / 100, 2),
                    Fat = Math.Round(pr.ProductNutrition.FatPer100Grams * pr.Grams / 100, 2),
                    Carbohydrates = Math.Round(pr.ProductNutrition.CarbohydratesPer100Grams * pr.Grams / 100, 2)
                }).ToList()
            };

            return Ok(recordResponse);
        }
        catch (UserIsNotAuthorizedException exception)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = exception.Message });
        }
        catch (UserDoesNotExistException exception)
        {
            return StatusCode(StatusCodes.Status404NotFound, new { message = exception.Message });
        }
    }

    [HttpPost("addNewProductRecord")]
    public async Task<ActionResult> AddProductToRecord(ProductRecordRequest productRecordRequest)
    {
        try
        {
            var user = await _userService.GetUserAsync();

            var currentDate = productRecordRequest.Date?.Date ?? DateTime.UtcNow.Date;

            var diary = await _context.Diaries
                .Include(d => d.Records)
                .FirstAsync(d => d.UserId == user.Id);

            var record = diary.Records.FirstOrDefault(r => r.Date == currentDate);
            if (record is null)
            {
                var userCurrentGoalType = await _userService.GetLastUsersGoalTypeLog(user.Id);

                var userCurrentActivityLevel = await _userService.GetLastUserActivityLevelLog(user.Id);

                var userWeightRecord = await _context.WeightRecords
                    .Include(w => w.User)
                    .Where(w => w.User.Id == user.Id)
                    .OrderByDescending(w => w.DateOfRecordCreated)
                    .Select(w => new WeightRecordResponse
                    {
                        Weight = w.Weight
                    })
                    .FirstAsync();

                var age = DateTime.Now.Year - user.DateOfBirth.Year;

                if (DateTime.Now.DayOfYear < user.DateOfBirth.DayOfYear) age--;

                var calculator = new CaloriesCalc(user.UserGender, age, user.Height, userWeightRecord.Weight,
                    userCurrentActivityLevel.ActivityLevel, userCurrentGoalType.Goal);

                record = Record.Create(currentDate,
                    calculator.CalculateDailyCalories(), calculator.CalculateDailyProtein(),
                    calculator.CalculateDailyFat()
                    , calculator.CalculateDailyCarbohydrates(), diary, userCurrentActivityLevel, userCurrentGoalType);

                await _context.Records.AddAsync(record);
            }

            var productNutrition = await _context.ProductNutritions
                .FirstOrDefaultAsync(p => p.Id == productRecordRequest.ProductNutritionId);

            if (productNutrition is null) return NotFound("Продукт не знайдено");

            var newProductRecord = ProductRecord.Create(record, productNutrition, productRecordRequest.ConsumedGrams);

            await _context.ProductRecords.AddAsync(newProductRecord);

            await _context.SaveChangesAsync();

            return Ok(newProductRecord.Id);
        }
        catch (UserIsNotAuthorizedException exception)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = exception.Message });
        }
        catch (UserDoesNotExistException exception)
        {
            return StatusCode(StatusCodes.Status404NotFound, new { message = exception.Message });
        }
    }

    [HttpPut("updateProductRecord")]
    public async Task<ActionResult> UpdateProductRecord([FromBody] ProductRecordUpdateRequest productRecordRequest)
    {
        try
        {
            var user = await _userService.GetUserAsync();

            var product = await _context.ProductRecords
                .Include(p => p.Record)
                .ThenInclude(p => p.Diary)
                .FirstOrDefaultAsync(p =>
                    p.Id == productRecordRequest.ProductRecordId && p.Record.Diary.User.Id == user.Id);

            if (product is null) return NotFound("Продукт не знайдено");

            product.Grams = Math.Round(productRecordRequest.ConsumedGrams, 2);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (UserIsNotAuthorizedException exception)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = exception.Message });
        }
        catch (UserDoesNotExistException exception)
        {
            return StatusCode(StatusCodes.Status404NotFound, new { message = exception.Message });
        }
    }

    [HttpDelete("deleteProductRecord/{id}")]
    public async Task<ActionResult> DeleteProductRecord(int id)
    {
        try
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
        catch (UserIsNotAuthorizedException exception)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = exception.Message });
        }
        catch (UserDoesNotExistException exception)
        {
            return StatusCode(StatusCodes.Status404NotFound, new { message = exception.Message });
        }
    }

    [HttpGet("getStatisticsByPeriod/{period}")]
    public async Task<ActionResult<List<PeriodStatisticsResponse>>> GetStatisticsByPeriodAsync(string period)
    {
        try
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
                    var calories = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => (int)Math.Round(pr.Grams / 100.0 * pr.ProductNutrition.CaloriesPer100Grams));
                    var proteins = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => pr.Grams / 100.0 * pr.ProductNutrition.ProteinPer100Grams);
                    var fats = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => pr.Grams / 100.0 * pr.ProductNutrition.FatPer100Grams);
                    var carbs = g.SelectMany(r => r.ProductRecords)
                        .Sum(pr => pr.Grams / 100.0 * pr.ProductNutrition.CarbohydratesPer100Grams);

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
                        ConsumedProteins = Math.Round(proteins, 2),
                        ConsumedFats = Math.Round(fats, 2),
                        ConsumedCarbohydrates = Math.Round(carbs, 2),
                        Status = status.ToString()
                    };
                })
                .OrderBy(d => d.Date)
                .ToList();

            return Ok(dailyStats);
        }
        catch (UserIsNotAuthorizedException exception)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = exception.Message });
        }
        catch (UserDoesNotExistException exception)
        {
            return StatusCode(StatusCodes.Status404NotFound, new { message = exception.Message });
        }
    }

    private static NormStatus GetNormStatus(
        int calories, int normCalories,
        double proteins, double normProteins,
        double fats, double normFats,
        double carbs, double normCarbohydrates)
    {
        var okCalories = calories >= normCalories;
        var okProteins = proteins >= normProteins;
        var okFats = fats >= normFats;
        var okCarbs = carbs >= normCarbohydrates;

        var overCalories = calories > normCalories * 1.15;
        var overProteins = proteins > normProteins * 1.15;
        var overFats = fats > normFats * 1.15;
        var overCarbs = carbs > normCarbohydrates * 1.15;

        if (overCalories || overProteins || overFats || overCarbs)
            return NormStatus.Exceeded;

        if (okCalories && okProteins && okFats && okCarbs)
            return NormStatus.Reached;

        return NormStatus.NotReached;
    }
}