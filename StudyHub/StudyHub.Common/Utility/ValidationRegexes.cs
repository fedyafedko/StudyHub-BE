namespace StudyHub.Common.Utility;
public static class ValidationRegexes
{
    public const string FullNameRegexes = @"^[A-Za-z\s]*$";
    public const string UpperCaseRegexes = @"[A-Z]+";
    public const string LowerCaseRegexes = @"[a-z]+";
    public const string NumberRegexes = @"[0-9]+";
    public const string SymbolsRegexes = @"[\!\?\*\.]+";
}
