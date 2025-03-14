#region Usings

#endregion

using UserService.Shared.Models;

namespace UserService.Shared.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
}
