#region Usings

using Microsoft.AspNetCore.Mvc;
using TSWMS.UserService.Shared.Interfaces;


#endregion

namespace TSWMS.UserService.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userService;

        public UserController(IUserManager userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsersAsync();

                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // TODO: Implement logging later through logging service.
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }
    }
}