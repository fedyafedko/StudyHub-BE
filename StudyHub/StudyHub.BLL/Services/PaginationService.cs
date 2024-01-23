using AutoMapper;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.User;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;

public class PaginationService : IPaginationService
{
    private readonly IMapper _mapper;

    public PaginationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public PageList Pagination(List<User> users, int page, int pageSize)
    {
        var pageList = new PageList();
        var pageUsers = users.Skip((page - 1) * pageSize).Take(pageSize);

        pageList.TotalCount = users.Count;
        pageList.TotalPages = (int)Math.Ceiling((decimal)users.Count / pageSize);
        pageList.Users = _mapper.Map<List<UserDTO>>(pageUsers);

        return pageList;
    }
}

public class PageList
{
    public List<UserDTO> Users { get; set; } = null!;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
