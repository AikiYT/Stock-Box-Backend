using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockBox.Identity.Context;
using StockBox.Identity.Models;
using StockBox.Identity.Services;
using System;

namespace StockBox.Identity.DependencyInjection
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection AddIdentityInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<StockBoxIdentityDbContext>(
                options =>
                    options.UseNpgsql(
                        configuration.GetConnectionString(
                            "IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;

                    options.User.RequireUniqueEmail = true;

                    options.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddEntityFrameworkStores<StockBoxIdentityDbContext>()
                .AddDefaultTokenProviders();

            // Application Cookie configuration
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "StockBox.Auth";

                options.LoginPath = "/api/auth/login";
                options.AccessDeniedPath = "/api/auth/access-denied";

                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;

                options.Cookie.HttpOnly = true;

                // Required for cross-site requests from the frontend
                options.Cookie.SameSite = SameSiteMode.None;

                // SameSite=None requires Secure
                options.Cookie.SecurePolicy =
                    CookieSecurePolicy.Always;

                // Return 401 instead of redirecting to a login page
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode =
                        StatusCodes.Status401Unauthorized;

                    return Task.CompletedTask;
                };

                // Return 403 instead of redirecting to an access-denied page
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode =
                        StatusCodes.Status403Forbidden;

                    return Task.CompletedTask;
                };
            });

            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}