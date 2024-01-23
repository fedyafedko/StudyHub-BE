using StudyHub.Entities;

namespace StudyHub.BLL.Services.Interfaces;
public interface IPaginationService
{
    PageList Pagination(List<User> users, int page, int pageSize);
}
