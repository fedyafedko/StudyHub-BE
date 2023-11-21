namespace StudyHub.Common.Exceptions;
public class UserUnauthorizationException : Exception
{
    public UserUnauthorizationException
        (string? message) : base(message) { }
}
