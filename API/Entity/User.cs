using Microsoft.AspNetCore.Identity;
using NutriTrack.Entity.Enums;

namespace NutriTrack.Entity;

public class User : IdentityUser<int>
{
    public Gender UserGender { get; set; }

    public DateTime DateOfBirth { get; set; }
    public int Height { get; set; }
}