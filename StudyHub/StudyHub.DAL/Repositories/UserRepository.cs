using Microsoft.EntityFrameworkCore;
using StudyHub.DAL.EF;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.DAL.Repositories;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    public User? FindByLogin(string login)
    {
        return _table.FirstOrDefault(user => user.Email == login);
    }
    public async Task<User?> FindByLoginAsync(string login)
    {
        return await _table.FirstOrDefaultAsync(user => user.Email == login);
    }
}
