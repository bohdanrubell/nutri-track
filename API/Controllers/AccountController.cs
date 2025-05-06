using System.Globalization;
using System.Net;
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
using NutriTrack.Services.Interfaces;

namespace NutriTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly TokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly UserService _userService;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public AccountController(ApplicationDbContext context, UserManager<User> userManager, TokenService tokenService,
        UserService userService, IEmailSender emailSender, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _tokenService = tokenService;
        _userService = userService;
        _emailSender = emailSender;
        _configuration = configuration;
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return Ok(); // не палимо юзера

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebUtility.UrlEncode(token);

        var callbackUrl = $"{_configuration["FrontendUrl"]}/reset-password?email={dto.Email}&token={encodedToken}";
        var message = $"<p>Щоб скинути пароль, натисни <a href='{callbackUrl}'>тут</a>.</p>";

        await _emailSender.SendAsync(dto.Email, "Відновлення паролю NutriTrack", message);
        return Ok("Лист надіслано (якщо email існує)");
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return BadRequest("Користувача не знайдено");

        var decodedToken = WebUtility.UrlDecode(dto.Token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Пароль змінено успішно");
    }

    
    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Username);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Unauthorized();

        return new UserResponse
        {
            Id = user.Id.ToString(),
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
                DateOfBirth = user.DateOfBirth.ToString("dd MMMM yyyy", new CultureInfo("uk-UA"))
                ,
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
                userCurrentActivityLevel = newActivityLevelLog;
            }

            if (userCurrentGoalType.Goal.Name != request.CurrentGoalType)
            {
                var initialGoalTypeLog = GoalTypeLog.Create(timeProvider, goal, user);
                await _context.GoalTypeLogs.AddAsync(initialGoalTypeLog, cancellationToken);
                userCurrentGoalType = initialGoalTypeLog;
            }
            
            await _userManager.UpdateAsync(user);
            
            var todayRecord = await _context.Records
                .FirstAsync(r => r.Date.Date == DateTime.Today 
                                          && r.Diary.User.Id == user.Id, 
                    cancellationToken);
            
            var age = DateTime.Now.Year - user.DateOfBirth.Year;
            if (DateTime.Now < user.DateOfBirth.AddYears(age)) age--;

            var weight = await _userService.GetLatestWeightRecordAsync(user.Id);
            
            var calculator = new CaloriesCalc(user.UserGender, age, user.Height, weight,
                userCurrentActivityLevel.ActivityLevel, userCurrentGoalType.Goal);
            
            todayRecord.DailyCalories = calculator.CalculateDailyCalories();
            todayRecord.DailyProtein = calculator.CalculateDailyProtein();
            todayRecord.DailyFat = calculator.CalculateDailyFat();
            todayRecord.DailyCarbohydrates = calculator.CalculateDailyCarbohydrates();
            
            await _context.SaveChangesAsync(cancellationToken);
            
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
            
            var todayRecord = await _context.Records
                .FirstAsync(r => r.Date.Date == DateTime.Today 
                                 && r.Diary.User.Id == user.Id, 
                    cancellationToken);
            
            var age = DateTime.Now.Year - user.DateOfBirth.Year;
            if (DateTime.Now < user.DateOfBirth.AddYears(age)) age--;
            
            var userCurrentGoalType = await _userService.GetLastUsersGoalTypeLog(user.Id);

            var userCurrentActivityLevel = await _userService.GetLastUserActivityLevelLog(user.Id);
            
            var calculator = new CaloriesCalc(user.UserGender, age, user.Height, newWeightRecord.Weight,
                userCurrentActivityLevel.ActivityLevel, userCurrentGoalType.Goal);
            
            todayRecord.DailyCalories = calculator.CalculateDailyCalories();
            todayRecord.DailyProtein = calculator.CalculateDailyProtein();
            todayRecord.DailyFat = calculator.CalculateDailyFat();
            todayRecord.DailyCarbohydrates = calculator.CalculateDailyCarbohydrates();
            
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
                Id = user.Id.ToString(),
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