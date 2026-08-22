using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockBox.Application.ViewModels.Identity;
using StockBox.Identity.Services;

namespace StockBox.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] LoginViewModel model)
        {
            var success =
                await _identityService.LoginAsync(
                    model,
                    HttpContext);

            if (!success)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            return Ok(new
            {
                message = "Login successful."
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _identityService.LogoutAsync(
                HttpContext);

            return Ok(new
            {
                message = "Logout successful."
            });
        }

        [HttpGet("access-denied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    message = "Access denied."
                });
        }
    }
}
