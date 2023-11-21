namespace StudyHub.Common.Exceptions;
public class AssignmentNotFoundException : Exception
{
    public AssignmentNotFoundException(string? message) : base(message)
    {
    }
}
