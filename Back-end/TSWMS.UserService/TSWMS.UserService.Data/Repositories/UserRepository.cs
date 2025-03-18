#region Usings

using Microsoft.EntityFrameworkCore;
using TSWMS.UserService.Shared.Interfaces;
using TSWMS.UserService.Shared.Models;

#endregion

namespace TSWMS.UserService.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _userDbContext;

    public UserRepository(UserDbContext userContext)
    {
        _userDbContext = userContext;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userDbContext.Users.ToListAsync();
    }
}
