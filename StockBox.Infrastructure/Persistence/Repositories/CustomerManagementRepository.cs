using Microsoft.EntityFrameworkCore;
using Stock_Box.Entities;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Infrastructure.Persistence.Repositories
{
    public class CustomerManagementRepository : ICustomerManagementRepository
    {
        private readonly StockBoxDbContext _context;

        public CustomerManagementRepository(StockBoxDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateCompleteCustomerAsync(
            Customer customer,
            CustomerServiceRecord? serviceRecord,
            Debt? debt)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Crear cliente
                await _context.Customers.AddAsync(customer);

                // Guardamos para obtener el Id del cliente
                await _context.SaveChangesAsync();

                // 2. Crear servicio si existe
                if (serviceRecord != null)
                {
                    serviceRecord.CustomerId = customer.Id;

                    await _context.CustomerServiceRecords
                        .AddAsync(serviceRecord);
                }

                // 3. Crear deuda si existe
                if (debt != null)
                {
                    debt.CustomerId = customer.Id;

                    await _context.Debts
                        .AddAsync(debt);
                }

                // Guardamos servicio y deuda
                await _context.SaveChangesAsync();

                // Confirmamos toda la operación
                await transaction.CommitAsync();

                return customer;
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<Customer?> GetCompleteCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.ServiceRecords)
                .Include(c => c.Debts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Customer>> GetAllCompleteCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.ServiceRecords)
                .Include(c => c.Debts)
                .ToListAsync();
        }
    }
}
