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

        Task LogoutAsync(
            HttpContext httpContext);

        Task<(bool Success, string Message)> CreateUserAsync(
            CreateUserViewModel model);

        Task<List<UserViewModel>> GetUsersAsync();

        Task<List<string>> GetRolesAsync();

        Task<bool> AssignRoleAsync(
            string userId,
            string roleName);

        Task<(bool Success, string Message)> UpdateUserAsync(
            string userId,
            UpdateUserViewModel model);

        Task<(bool Success, string Message)> DeleteUserAsync(
            string userId);
    }
}