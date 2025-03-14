﻿using TSWMS.UserService.Shared.Interfaces;
using UserService.Shared.Interfaces;
using UserService.Shared.Models;

namespace TSWMS.UserService.Business.Services;

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
