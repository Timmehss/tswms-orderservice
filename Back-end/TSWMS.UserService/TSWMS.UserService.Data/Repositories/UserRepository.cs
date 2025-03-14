#region Usings

using Microsoft.EntityFrameworkCore;
using UserService.Shared.Interfaces;
using UserService.Shared.Models;

#endregion

namespace UserService.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _userContext;

    public UserRepository(UserDbContext userContext)
    {
        _userContext = userContext;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userContext.Users.ToListAsync();
    }
}
