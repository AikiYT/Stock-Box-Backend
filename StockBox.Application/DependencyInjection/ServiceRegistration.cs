using Microsoft.Extensions.DependencyInjection;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerServiceRecordService, CustomerServiceRecordService>();
            services.AddScoped<IDebtService, DebtService>();
            services.AddScoped<ICustomerManagementService, CustomerManagementService>();
            return services;
        }
    }
}