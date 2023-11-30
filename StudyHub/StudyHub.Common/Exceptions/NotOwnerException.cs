namespace StudyHub.Common.Exceptions;
public class NotOwnerException : Exception
{
    public NotOwnerException(string? message)
        : base(message) { }
}
