using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriTrack.Data;
using NutriTrack.DTO;
using NutriTrack.Entity;
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
            Token = await tokenService.GenerateToken(user)
        };
    }
}

