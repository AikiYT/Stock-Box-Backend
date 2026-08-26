using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockBox.Application.ViewModels.Identity;
using StockBox.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> LoginAsync(
            LoginViewModel model,
            HttpContext httpContext)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null || !user.IsActive)
                return false;

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            return result.Succeeded;
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(bool Success, string Message)> CreateUserAsync(
            CreateUserViewModel model)
        {
            var existingUser = await _userManager.FindByNameAsync(
                model.UserName);

            if (existingUser != null)
                return (false, "Username already exists.");

            var existingEmail = await _userManager.FindByEmailAsync(
                model.Email);

            if (existingEmail != null)
                return (false, "Email already exists.");

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(
                user,
                model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                return (false, errors);
            }

            if (!string.IsNullOrWhiteSpace(model.Role))
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _userManager.DeleteAsync(user);

                    return (
                        false,
                        $"Role '{model.Role}' does not exist.");
                }

                await _userManager.AddToRoleAsync(
                    user,
                    model.Role);
            }

            return (true, "User created successfully.");
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Roles = roles.ToList()
                });
            }

            return result;
        }

        public async Task<bool> AssignRoleAsync(
            string userId,
            string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            if (!await _roleManager.RoleExistsAsync(roleName))
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(
                    user,
                    currentRoles);
            }

            var result = await _userManager.AddToRoleAsync(
                user,
                roleName);

            return result.Succeeded;
        }

        public async Task<List<string>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Select(r => r.Name!)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(
    string userId,
    UpdateUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found.");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.IsActive = model.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    " ",
                    result.Errors.Select(e => e.Description));

                return (false, errors);
            }

            return (true, "User updated successfully.");
        }
        public async Task<(bool Success, string Message)> DeleteUserAsync(
    string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    " ",
                    result.Errors.Select(e => e.Description));

                return (false, errors);
            }

            return (true, "User deleted successfully.");
        }
    }
}
