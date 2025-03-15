#region Usings

using Microsoft.EntityFrameworkCore;
using TSWMS.UserService.Shared.Interfaces;
using TSWMS.UserService.Shared.Models;



#endregion

namespace TSWMS.UserService.Data.Repositories;

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
