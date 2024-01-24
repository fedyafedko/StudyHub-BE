namespace StudyHub.Common.DTO.User;

public class PageList<T>
{
    public List<T> Users { get; set; } = null!;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
