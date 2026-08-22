using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.services;
using StockBox.Infrastructure.Persistence.Context;
using StockBox.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Infrastructure.Persistence.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<StockBoxDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerServiceRecordRepository, CustomerServiceRecordRepository>();
            services.AddScoped<IDebtRepository, DebtRepository>();

            return services;
        }
    }
}