using TSWMS.UserService.Shared.Models;

namespace TSWMS.UserService.Shared.Interfaces;

public interface IUserManager
{
    Task<IEnumerable<User>> GetUsersAsync();
}
