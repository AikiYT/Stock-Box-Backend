using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StockBox.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Identity.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<
                    RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<
                    UserManager<ApplicationUser>>();

            string[] roles =
            {
                "SuperAdmin",
                "Admin",
                "Employee"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            var superAdmin =
                await userManager.FindByNameAsync("superadmin");

            if (superAdmin == null)
            {
                superAdmin = new ApplicationUser
                {
                    UserName = "superadmin",
                    Email = "superadmin@stockbox.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(
                    superAdmin,
                    "SuperAdmin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        superAdmin,
                        "SuperAdmin");
                }
            }
        }
    }
}
