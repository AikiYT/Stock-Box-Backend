using Microsoft.AspNetCore.Http;
using StockBox.Application.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Identity.Services
{
    public interface IIdentityService
    {
        Task<bool> LoginAsync(
            LoginViewModel model,
            HttpContext httpContext);

        Task LogoutAsync(HttpContext httpContext);

        Task<(bool Success, string Message)> CreateUserAsync(
            CreateUserViewModel model);

        Task<List<UserViewModel>> GetUsersAsync();

        Task<bool> AssignRoleAsync(
            string userId,
            string roleName);

        Task<List<string>> GetRolesAsync();
    }
}