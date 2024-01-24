using StudyHub.Common.DTO.User;

namespace StudyHub.BLL.Extensions;
public static class PaginationExtension
{
    public static PageList<UserDTO> Pagination(this List<UserDTO> users, int page, int pageSize)
    {
        var pageUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var pageList = new PageList<UserDTO>()
        {
            TotalCount = users.Count,
            TotalPages = (int)Math.Ceiling((decimal)users.Count / pageSize),
            Users = pageUsers
        };

        return pageList;
    }
}
