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

public class AccountController(UserManager<User> userManager, TokenService tokenService, ApplicationDbContext context) : BaseApiController
{
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
            DateOfBirth = user.DateOfBirth,
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
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

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
        if (goal == null || activityLevel == null)
        {
            return NotFound("Goal or activity not found");
        }
        
        var initialGoalTypeLog = new GoalTypeLog
        {
            Date = DateTime.Now.Date,
            Goal = goal,
            User = user,
        };
        
        await context.GoalTypeLogs.AddAsync(initialGoalTypeLog);
        
        var initialActivityLevelLog = new ActivityLevelLog
        {
            Date = DateTime.Now.Date,
            ActivityLevel = activityLevel,
            User = user,
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
    
    private async Task<GoalType?> GetGoalTypeByName(string name)
    {
        return await context.GoalTypes.FirstOrDefaultAsync(g => g.Name == name);
    }
    private async Task<ActivityLevel?> GetActivityLevelByName(string name)
    {
        return await context.ActivityLevels.FirstOrDefaultAsync(g => g.Name == name);
    }
}

