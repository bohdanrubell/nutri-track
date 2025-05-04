namespace NutriTrack.Entities;

public class Diary
{
    public int Id { get; set; }
    public DateTime DateDiaryCreated { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    public List<Record> Records { get; set; }

    public static Diary Create(TimeProvider timeProvider, User user)
    {
        var diary = new Diary
        {
            DateDiaryCreated = timeProvider.GetUtcNow().Date,
            User = user
        };
        
        return diary;
    }
}