using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockBox.Application.ViewModels.Identity;
using StockBox.Identity.Services;

namespace StockBox.Api.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UsersController(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users =
                await _identityService.GetUsersAsync();

            return Ok(users);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserViewModel model)
        {
            var result =
                await _identityService.CreateUserAsync(
                    model);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }

        [HttpGet("roles")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetRoles()
        {
            var roles =
                await _identityService.GetRolesAsync();

            return Ok(roles);
        }

        [HttpPost("{userId}/role")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssignRole(
            string userId,
            [FromBody] string roleName)
        {
            var result =
                await _identityService.AssignRoleAsync(
                    userId,
                    roleName);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Unable to assign role."
                });
            }

            return Ok(new
            {
                message = "Role assigned successfully."
            });
        }
        [HttpPut("{userId}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> UpdateUser(
    string userId,
    [FromBody] UpdateUserViewModel model)
        {
            var result =
                await _identityService.UpdateUserAsync(
                    userId,
                    model);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }
        [HttpDelete("{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteUser(
    string userId)
        {
            var result =
                await _identityService.DeleteUserAsync(
                    userId);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }
    }
}  