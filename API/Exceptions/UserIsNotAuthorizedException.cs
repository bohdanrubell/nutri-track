namespace NutriTrack.Exceptions;

public class UserIsNotAuthorizedException : AppException
{
    public UserIsNotAuthorizedException() 
        : base("Користувач не авторизований!") 
    {
    }
}