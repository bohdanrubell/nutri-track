namespace NutriTrack.Entities;

public class Diary
{
    public int Id { get; set; }
    public DateTime DateDiaryCreated { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}