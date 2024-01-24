using AutoMapper;
using StudyHub.Common.DTO.User;
using StudyHub.Entities;

namespace StudyHub.BLL.Extensions;
public static class PaginationExtension
{
    public static PageList<UserDTO> Pagination(this List<User> users, IMapper mapper, int page, int pageSize)
    {
        var pageUsers = users.Skip((page - 1) * pageSize).Take(pageSize);

        var pageList = new PageList<UserDTO>()
        {
            TotalCount = users.Count,
            TotalPages = (int)Math.Ceiling((decimal)users.Count / pageSize),
            Users = mapper.Map<List<UserDTO>>(pageUsers)
        };

        return pageList;
    }
}
