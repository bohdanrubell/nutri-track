using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NutriTrack.Entities;
using NutriTrack.Entity;

namespace NutriTrack.Services;

public class TokenService(UserManager<User> userManager, IConfiguration configuration)
{
    public async Task<string> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new("email", user.Email),
            new("userId", user.Id.ToString()),
            new("userName", user.UserName)
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim("role", role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:TokenKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenOptions = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials:creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}