using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.Entities;
using NutriTrack.Entity;
using NutriTrack.Entity.Enums;
using NutriTrack.Services;

namespace NutriTrack.Controllers;

public class AccountController(UserManager<User> userManager, TokenService tokenService, ApplicationDbContext context)
    : BaseApiController
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly UserService _userService;
    
    public AccountController(ApplicationDbContext context, UserManager<User> userManager, TokenService tokenService, UserService userService)
    {
        _context = context;
        _userManager = userManager;
        _tokenService = tokenService;
        _userService = userService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.Username);
        if (user is null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            return Unauthorized();

        var latestWeightRecord = await context.WeightRecords
            .Include(w => w.User)
            .Where(w => w.User.Id == user.Id)
            .OrderByDescending(w => w.DateOfRecordCreated)
            .FirstOrDefaultAsync();

        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Height = user.Height,
            Weight = latestWeightRecord?.Weight ?? 0,
            Gender = user.UserGender.ToString(),
            DateOfBirth = user.DateOfBirth.Date.ToString("dd/MM/yyyy"),
            Token = await tokenService.GenerateToken(user)
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            UserGender = Enum.Parse<Gender>(registerDto.Gender),
            DateOfBirth = registerDto.DateOfBirth,
            Height = registerDto.Height
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Description);

            return ValidationProblem();
        }

        await userManager.AddToRoleAsync(user, "User");

        var weightRecord = new WeightRecord
        {
            DateOfRecordCreated = DateTime.Now.Date,
            User = user,
            Weight = registerDto.Weight
        };

        await context.WeightRecords.AddAsync(weightRecord);


        var goal = await GetGoalTypeByName(registerDto.Goal);
        var activityLevel = await GetActivityLevelByName(registerDto.Activity);
        if (goal == null || activityLevel == null) return NotFound("Goal or activity not found");

        var initialGoalTypeLog = new GoalTypeLog
        {
            Date = DateTime.Now.Date,
            Goal = goal,
            User = user
        };

        await context.GoalTypeLogs.AddAsync(initialGoalTypeLog);

        var initialActivityLevelLog = new ActivityLevelLog
        {
            Date = DateTime.Now.Date,
            ActivityLevel = activityLevel,
            User = user
        };

        await context.ActivityLevelLogs.AddAsync(initialActivityLevelLog);

        var initialDiary = new Diary
        {
            DateDiaryCreated = DateTime.Now.Date,
            User = user
        };

        await context.Diaries.AddAsync(initialDiary);

        await context.SaveChangesAsync();

        Console.WriteLine($"User {user.UserName} with id: {user.Id} has been created");

        return StatusCode(201);
    }

    /*[Authorize(Roles = "User")]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetUserChar()
    {
        var user = await userManager.Users
            .Where(x => x. == User.Identity.)

    }*/

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserCharacteristicsResponse>> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return NotFound("Користувач не авторизований.");

        var user = await userManager.FindByIdAsync(userId);

        if (user == null) return NotFound("Користувача не знайдено.");

        var userCurrentGoalType = await context.GoalTypeLogs
            .Include(u => u.User)
            .Include(goalTypeLog => goalTypeLog.Goal)
            .Where(u => u.User.Id == user.Id)
            .OrderByDescending(u => u.Date)
            .FirstAsync();

        var userCurrentActivityLevel = await context.ActivityLevelLogs
            .Include(u => u.User)
            .Include(activityLevelLog => activityLevelLog.ActivityLevel)
            .Where(u => u.User.Id == user.Id)
            .OrderByDescending(u => u.Date)
            .FirstAsync();

        var usersWeightRecords = await context.WeightRecords
            .Include(w => w.User)
            .Where(w => w.User.Id == user.Id)
            .OrderByDescending(w => w.DateOfRecordCreated)
            .Select(w => new WeightRecordResponse
            {
                Id = w.Id,
                Date = w.DateOfRecordCreated.ToString("dd/MM/yyyy"),
                Weight = w.Weight,
            })
            .ToListAsync();

        var latestWeightRecord = usersWeightRecords.First();

        var age = DateTime.Now.Year - user.DateOfBirth.Year;

        if (DateTime.Now.DayOfYear < user.DateOfBirth.DayOfYear) age--;

        var calculator = new CaloriesCalc(user.UserGender, age, user.Height, latestWeightRecord.Weight,
            userCurrentActivityLevel.ActivityLevel, userCurrentGoalType.Goal);

        var dailyNutritionsResponse = new DailyNutritionsResponse
        {
            DailyCalories = calculator.CalculateDailyCalories(),
            DailyProtein = calculator.CalculateDailyProtein(),
            DailyFat = calculator.CalculateDailyFat(),
            DailyCarbohydrates = calculator.CalculateDailyCarbohydrates()
        };

        var userCharResponse = new UserCharacteristicsResponse
        {
            Sex = user.UserGender.ToString(),
            Age = age,
            Height = user.Height,
            CurrentGoalType = userCurrentGoalType.Goal.Name,
            CurrentActivityLevel = userCurrentActivityLevel.ActivityLevel.Name,
            DailyNutritions = dailyNutritionsResponse,
            WeightRecords = usersWeightRecords
        };
        
        return Ok(userCharResponse);
    }

    private async Task<GoalType?> GetGoalTypeByName(string name)
    {
        return await context.GoalTypes.FirstOrDefaultAsync(g => g.Name == name);
    }

    private async Task<ActivityLevel?> GetActivityLevelByName(string name)
    {
        return await context.ActivityLevels.FirstOrDefaultAsync(g => g.Name == name);
    }
}