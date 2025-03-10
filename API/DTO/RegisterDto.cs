namespace NutriTrack.DTO;

public class RegisterDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Goal { get; set; }
    public string Activity { get; set; }
}