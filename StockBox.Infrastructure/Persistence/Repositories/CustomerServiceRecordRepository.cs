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
    public class CustomerServiceRecordRepository : ICustomerServiceRecordRepository
    {
        private readonly StockBoxDbContext _context;

        public CustomerServiceRecordRepository(StockBoxDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerServiceRecord> AddAsync(CustomerServiceRecord record)
        {
            await _context.CustomerServiceRecords.AddAsync(record);
            await _context.SaveChangesAsync();

            return record;
        }

        public async Task<CustomerServiceRecord?> GetByIdAsync(int id)
        {
            return await _context.CustomerServiceRecords
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<CustomerServiceRecord>> GetAllAsync()
        {
            return await _context.CustomerServiceRecords
                .Include(r => r.Customer)
                .ToListAsync();
        }

        public async Task<List<CustomerServiceRecord>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.CustomerServiceRecords
                .Include(r => r.Customer)
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task UpdateAsync(CustomerServiceRecord record)
        {
            _context.CustomerServiceRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CustomerServiceRecord record)
        {
            _context.CustomerServiceRecords.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}