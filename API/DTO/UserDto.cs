namespace NutriTrack.DTO;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string Token { get; set; }
}