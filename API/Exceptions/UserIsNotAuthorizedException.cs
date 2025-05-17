namespace NutriTrack.Exceptions;

public class UserIsNotAuthorizedException : AppException
{
    public UserIsNotAuthorizedException(string message = "Користувач не авторизований!") 
        : base(message) 
    {
    }
}