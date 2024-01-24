using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common;
using StudyHub.Common.DTO.User;
using StudyHub.Common.Requests;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<PageList<UserDTO>> GetStudents(SearchRequest request)
    {
        var users = await _userManager.GetUsersInRoleAsync(UserRole.Student.Value);

        var searchUsers = SearchStudent(users, request);

        var result = searchUsers.Pagination(_mapper, request.Page, request.PageSize);

        return result;
    }

    private List<User> SearchStudent(IEnumerable<User> users, SearchRequest request)
    {
        if (!string.IsNullOrEmpty(request.FullName))
            users = users.Where(u => u.FullName
                .ToLower()
                .Contains(request.FullName.ToLower()));

        if (!string.IsNullOrEmpty(request.Group))
            users = users.Where(u => u.Group != null && u.Group
                .ToLower()
                .Contains(request.Group.ToLower()));

        if (!string.IsNullOrEmpty(request.Email))
            users = users.Where(u => u.Email != null && u.Email
                .ToLower()
                .Contains(request.Email.ToLower()));
        
        return users.ToList();
    }
}
