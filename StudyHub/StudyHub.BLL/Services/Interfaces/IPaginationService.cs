using StudyHub.Common.DTO.User;
using StudyHub.Entities;

namespace StudyHub.BLL.Services.Interfaces;

public interface IPaginationService
{
    PageList Pagination(List<User> users, int page, int pageSize);
}
