namespace NutriTrack.Exceptions;

public class UserDoesNotExistException : ApplicationException
{
    public UserDoesNotExistException(string userId)
    : base($"Користувача з ID: {userId} не було знайдено!")
    {
        
    }
}