namespace NutriTrack.Exceptions;

public class UserDoesNotExistException : AppException
{
    public UserDoesNotExistException(string userId)
    : base($"Користувача з ID: {userId} не було знайдено!")
    {
        
    }
}