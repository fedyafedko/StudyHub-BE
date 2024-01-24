namespace StudyHub.Common.DTO.User;

public class PageList
{
    public List<UserDTO> Users { get; set; } = null!;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
