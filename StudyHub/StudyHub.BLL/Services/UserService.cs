using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common;
using StudyHub.Common.Configs;
using StudyHub.Common.DTO.User;
using StudyHub.Common.DTO.User.Student;
using StudyHub.Common.DTO.UserInvitation;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Requests;
using StudyHub.Common.Response;
using StudyHub.Entities;

namespace StudyHub.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly AvatarConfig _avatarConfig;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public UserService(
        UserManager<User> userManager,
        IOptions<AvatarConfig> avatarConfig,
        IMapper mapper,
        IWebHostEnvironment env)
    {
        _userManager = userManager;
        _mapper = mapper;
        _avatarConfig = avatarConfig.Value;
        _env = env;
    }

    public async Task<PageList<StudentDTO>> GetStudentsAsync(SearchRequest request)
    {
        var users = await _userManager.GetUsersInRoleAsync(UserRole.Student.Value);

        var searchUsers = _mapper.Map<List<StudentDTO>>(SearchStudent(users, request));

        var result = searchUsers.Pagination(request.Page, request.PageSize);

        return result;
    }

    public async Task<UserDTO> UpdateUserAsync(Guid userId, UpdateUserDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User with such ID does not exist in the database");

        _mapper.Map(dto, user);

        await _userManager.UpdateAsync(user);

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<AvatarResponse> UploadAvatarAsync(Guid userId, IFormFile avatar)
    {
        var contentPath = _env.ContentRootPath;
        var path = Path.Combine(contentPath, _avatarConfig.Folder);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var ext = Path.GetExtension(avatar.FileName);

        if (!_avatarConfig.FileExtensions.Contains(ext))
            throw new IncorrectParametersException("Invalid file extension");

        var newFileName = $"{userId}{ext}";
        var filePath = Path.Combine(path, newFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await avatar.CopyToAsync(stream);

        var result = new AvatarResponse
        {
            Path = string.Format(_avatarConfig.Path, newFileName)
        };

        return result;
    }

    public bool DeleteAvatar(Guid userId)
    {
        var contentPath = _env.ContentRootPath;
        var path = Path.Combine(contentPath, _avatarConfig.Folder);

        var file = Directory.GetFiles(path).FirstOrDefault(x => x.Contains(userId.ToString()));

        if (!File.Exists(file))
            throw new NotFoundException("File not found");

        File.Delete(file);

        return true;
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
