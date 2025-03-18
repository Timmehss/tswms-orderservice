using TSWMS.UserService.Shared.Interfaces;
using TSWMS.UserService.Shared.Models;

namespace TSWMS.UserService.Business.Managers;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;

    public UserManager(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _userRepository.GetUsers();
    }
}
