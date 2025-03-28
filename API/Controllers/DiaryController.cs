using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.DTO.ProductRecord;
using NutriTrack.DTO.Statistics;
using NutriTrack.DTO.User;
using NutriTrack.Entities;
using NutriTrack.Exceptions;
using NutriTrack.Services;

namespace NutriTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiaryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly TimeProvider _timeProvider;
    private readonly UserManager<User> _userManager;
    private readonly UserService _userService;

    public DiaryController(ApplicationDbContext context, UserManager<User> userManager, TimeProvider timeProvider,
        UserService userService)
    {
        _context = context;
        _userManager = userManager;
        _timeProvider = timeProvider;
        _userService = userService;
    }

    [Authorize]
    [HttpGet("getRecordByDate/{date}")]
    public async Task<ActionResult<DairyRecordResponse>> GetRecordByDate(DateTime date)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return NotFound("Користувач не авторизований.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return NotFound("Користувача не знайдено.");

        var record = await _context.Records
            .Where(r => r.Diary.User.Id == user.Id && r.Date.Date == date.Date)
            .Include(r => r.ProductRecords)
            .ThenInclude(pr => pr.ProductNutrition)
            .FirstOrDefaultAsync();

        if (record == null)
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
                Calories = pr.ProductNutrition.CaloriesPer100Grams * pr.Grams / 100,
                Protein = pr.ProductNutrition.ProteinPer100Grams * pr.Grams / 100,
                Fat = pr.ProductNutrition.FatPer100Grams * pr.Grams / 100,
                Carbohydrates = pr.ProductNutrition.CarbohydratesPer100Grams * pr.Grams / 100
            }).ToList()
        };

        return Ok(recordResponse);
    }
    
    [Authorize]
    [HttpPost("addNewProductRecord")]
    public async Task<ActionResult> AddProductToRecord([FromBody] ProductRecordRequest productRecordRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return NotFound("Користувач не авторизований.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return NotFound("Користувача не знайдено.");

        var currentDate = DateTime.UtcNow.Date;

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

            record = Record.Create(_timeProvider,
                calculator.CalculateDailyCalories(), calculator.CalculateDailyProtein(), calculator.CalculateDailyFat()
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

    [Authorize]
    [HttpPut("updateProductRecord")]
    public async Task<ActionResult> UpdateProductRecord([FromBody] ProductRecordUpdateRequest productRecordRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return NotFound("Користувач не авторизований.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return NotFound("Користувача не знайдено.");

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


    [Authorize]
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
            return Unauthorized(new { exception.Message });
        }
        catch (UserDoesNotExistException exception)
        {
            return NotFound(new { exception.Message });
        }
    }

    [Authorize]
    [HttpGet("getStatisticsPeriodByPeriod")]
    public async Task<ActionResult<List<PeriodStatisticsResponse>>> GetStatisticsByPeriodAsync([FromForm]ConsumptionPeriodRequest period)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found.");

        var lastActivity = await _userService.GetLastUserActivityLevelLog(user.Id);
        var lastGoal = await _userService.GetLastUsersGoalTypeLog(user.Id);
        
        var records = await _context.Records
            .Include(r => r.ProductRecords)
            .ThenInclude(pr => pr.ProductNutrition)
            .Where(r =>
                r.Date >= period.StartOfPeriod &&
                r.Date <= period.EndOfPeriod &&
                r.ActivityLog.ActivityId == lastActivity.ActivityId &&
                r.GoalLog.GoalTypeId == lastGoal.GoalTypeId)
            .ToListAsync();

        var dailyStats = records
            .GroupBy(r => r.Date.Date)
            .Select(g => new PeriodStatisticsResponse
            {
                Date = g.Key,
                Calories = g.SelectMany(r => r.ProductRecords).Sum(pr => pr.Grams / 100.0 * pr.ProductNutrition.CaloriesPer100Grams),
                Proteins = g.SelectMany(r => r.ProductRecords).Sum(pr => pr.Grams / 100.0 * pr.ProductNutrition.ProteinPer100Grams),
                Fats = g.SelectMany(r => r.ProductRecords).Sum(pr => pr.Grams / 100.0 * pr.ProductNutrition.FatPer100Grams),
                Carbohydrates = g.SelectMany(r => r.ProductRecords).Sum(pr => pr.Grams / 100.0 * pr.ProductNutrition.CarbohydratesPer100Grams)
            })
            .OrderBy(d => d.Date)
            .ToList();

        return Ok(dailyStats);
    }


}