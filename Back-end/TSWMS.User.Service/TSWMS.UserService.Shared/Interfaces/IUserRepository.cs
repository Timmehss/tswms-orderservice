#region Usings

#endregion

using TSWMS.UserService.Shared.Models;

namespace TSWMS.UserService.Shared.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
}
