using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.DTO.User;
using NutriTrack.Entities;
using NutriTrack.Entity;
using NutriTrack.Entity.Enums;
using NutriTrack.Exceptions;
using NutriTrack.Services;

namespace NutriTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly TokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly UserService _userService;

    public AccountController(ApplicationDbContext context, UserManager<User> userManager, TokenService tokenService,
        UserService userService)
    {
        _context = context;
        _userManager = userManager;
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Username);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Unauthorized();

        return new UserResponse
        {
            Id = user.Id,
            Username = user.UserName!,
            Token = await _tokenService.GenerateToken(user)
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest registerRequest, TimeProvider timeProvider)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        registerRequest.Gender = registerRequest.Gender == "Чоловік" ? "Male" : "Female";
        
        var user = new User
        {
            UserName = registerRequest.Username,
            Email = registerRequest.Email,
            UserGender = Enum.Parse<Gender>(registerRequest.Gender),
            DateOfBirth = registerRequest.DateOfBirth,
            Height = registerRequest.Height
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Description);

            return ValidationProblem();
        }

        await _userManager.AddToRoleAsync(user, "User");

        var weightRecord = WeightRecord.Create(timeProvider, registerRequest.Weight, user);
        await _context.WeightRecords.AddAsync(weightRecord);

        var goal = await GetGoalTypeByName(registerRequest.Goal);

        if (goal is null) return NotFound($"Ціль {registerRequest.Goal} не знайдено в базі даних");

        var activityLevel = await GetActivityLevelByName(registerRequest.Activity);

        if (activityLevel is null) return NotFound($"Ціль {registerRequest.Goal} не знайдено в базі даних");

        var initialGoalTypeLog = GoalTypeLog.Create(timeProvider, goal, user);
        await _context.GoalTypeLogs.AddAsync(initialGoalTypeLog);

        var initialActivityLevelLog = ActivityLevelLog.Create(timeProvider, activityLevel, user);
        await _context.ActivityLevelLogs.AddAsync(initialActivityLevelLog);

        var initialDiary = Diary.Create(timeProvider, user);
        await _context.Diaries.AddAsync(initialDiary);

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return StatusCode(201);
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserCharacteristicsResponse>> GetCurrentUserCharacteristics()
    {
        try
        {
            var user = await _userService.GetUserAsync();

            var userCurrentGoalType = await _userService.GetLastUsersGoalTypeLog(user.Id);

            var userCurrentActivityLevel = await _userService.GetLastUserActivityLevelLog(user.Id);

            var usersWeightRecords = await _context.WeightRecords
                .Include(w => w.User)
                .Where(w => w.User.Id == user.Id)
                .OrderByDescending(w => w.DateOfRecordCreated)
                .Select(w => new WeightRecordResponse
                {
                    Date = w.DateOfRecordCreated.ToString("dd/MM/yyyy"),
                    Weight = w.Weight
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
                Gender = user.UserGender.ToString(),
                DateOfBirth = user.DateOfBirth.ToString("dd/MM/yyyy"),
                Age = age,
                Height = user.Height,
                CurrentGoalType = userCurrentGoalType.Goal.Name,
                CurrentActivityLevel = userCurrentActivityLevel.ActivityLevel.Name,
                DailyNutritions = dailyNutritionsResponse,
                WeightRecords = usersWeightRecords
            };
            return Ok(userCharResponse);
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
    
    [HttpPut("profile")]
    public async Task<ActionResult> UpdateUserProfile(UpdateUserProfileRequest request, TimeProvider timeProvider,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.GetUserAsync();

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            user.UserGender = Enum.Parse<Gender>(request.Gender);
            user.Height = request.Height;

            var goal = await GetGoalTypeByName(request.CurrentGoalType);

            if (goal is null) return NotFound($"Ціль {request.CurrentGoalType} не знайдено в базі даних");

            var activityLevel = await GetActivityLevelByName(request.CurrentActivityLevel);

            if (activityLevel is null) return NotFound($"Ціль {request.CurrentActivityLevel} не знайдено в базі даних");

            var userCurrentGoalType = await _userService.GetLastUsersGoalTypeLog(user.Id);

            var userCurrentActivityLevel = await _userService.GetLastUserActivityLevelLog(user.Id);

            if (userCurrentActivityLevel.ActivityLevel.Name != request.CurrentActivityLevel)
            {
                var newActivityLevelLog = ActivityLevelLog.Create(timeProvider, activityLevel, user);
                await _context.ActivityLevelLogs.AddAsync(newActivityLevelLog, cancellationToken);
            }

            if (userCurrentGoalType.Goal.Name != request.CurrentGoalType)
            {
                var initialGoalTypeLog = GoalTypeLog.Create(timeProvider, goal, user);
                await _context.GoalTypeLogs.AddAsync(initialGoalTypeLog, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            await _userManager.UpdateAsync(user);

            await transaction.CommitAsync(cancellationToken);

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
    
    [HttpPost("addWeightRecord")]
    public async Task<ActionResult> AddNewWeightRecord(TimeProvider timeProvider, WeightRecordRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.GetUserAsync();

            var newWeightRecord = WeightRecord.Create(timeProvider, request.Weight, user);
            await _context.WeightRecords.AddAsync(newWeightRecord, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

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
    
    [HttpGet("currentUser")]
    public async Task<ActionResult<UserResponse>> GetCurrentUser()
    {
        try
        {
            var user = await _userService.GetUserAsync();

            return new UserResponse
            {
                Id = user.Id,
                Username = user.UserName!,
                Token = await _tokenService.GenerateToken(user)
            };
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

    [HttpGet("activityLevels")]
    public async Task<ActionResult<List<ActivityLevelResponse>>> GetActivityLevels()
    {
        var activityLevels = await _context.ActivityLevels
            .Select(a => new ActivityLevelResponse
            {
                Id = a.Id,
                Name = a.Name
            }).ToListAsync();

        return Ok(activityLevels);
    }

    [HttpGet("goalTypes")]
    public async Task<ActionResult<List<GoalTypeResponse>>> GetGoalTypes()
    {
        var goalTypes = await _context.GoalTypes
            .Select(g => new GoalTypeResponse
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync();

        return Ok(goalTypes);
    }

    private async Task<GoalType?> GetGoalTypeByName(string name)
    {
        return await _context.GoalTypes.FirstOrDefaultAsync(g => g.Name == name);
    }

    private async Task<ActivityLevel?> GetActivityLevelByName(string name)
    {
        return await _context.ActivityLevels.FirstOrDefaultAsync(g => g.Name == name);
    }
}