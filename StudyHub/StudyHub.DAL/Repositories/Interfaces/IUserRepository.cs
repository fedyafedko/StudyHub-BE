using StudyHub.Entities;

namespace StudyHub.DAL.Repositories.Interfaces;
public interface IUserRepository : IRepository<User>
{
    User? FindByLogin(string login);
    Task<User?> FindByLoginAsync(string login);
}
