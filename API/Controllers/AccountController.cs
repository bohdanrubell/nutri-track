using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriTrack.Data;
using NutriTrack.DTO;
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
        
        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Height = user.Height,
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

        Console.WriteLine($"User {user.UserName} with id: {user.Id} has been created");
        
        return StatusCode(201);
    }
}

