using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Interfaces;
public interface IGetStudentService
{
    Task<PageList> GetStudents(SearchRequest request);
}
