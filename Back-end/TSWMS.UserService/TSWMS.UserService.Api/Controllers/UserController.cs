#region Usings

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TSWMS.UserService.Api.Dto;
using TSWMS.UserService.Shared.Interfaces;


#endregion

namespace TSWMS.UserService.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public UserController(IUserManager userService, IMapper mapper)
        {
            _userManager = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.GetUsersAsync();

                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }

                return Ok(_mapper.Map<List<UserDto>>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }
    }
}