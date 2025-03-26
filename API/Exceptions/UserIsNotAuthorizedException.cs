namespace NutriTrack.Exceptions;

public class UserIsNotAuthorizedException : ApplicationException
{
    public UserIsNotAuthorizedException() 
        : base("Користувач не авторизований!") 
    {
    }
}