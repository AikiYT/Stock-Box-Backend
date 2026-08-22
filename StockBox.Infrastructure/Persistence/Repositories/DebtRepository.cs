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
    public class DebtRepository : IDebtRepository
    {
        private readonly StockBoxDbContext _context;

        public DebtRepository(StockBoxDbContext context)
        {
            _context = context;
        }

        public async Task<Debt> AddAsync(Debt debt)
        {
            await _context.Debts.AddAsync(debt);
            await _context.SaveChangesAsync();

            return debt;
        }

        public async Task<Debt?> GetByIdAsync(int id)
        {
            return await _context.Debts
                .Include(d => d.Customer)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Debt>> GetAllAsync()
        {
            return await _context.Debts
                .Include(d => d.Customer)
                .ToListAsync();
        }

        public async Task<List<Debt>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Debts
                .Include(d => d.Customer)
                .Where(d => d.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Debt debt)
        {
            _context.Debts.Update(debt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Debt debt)
        {
            _context.Debts.Remove(debt);
            await _context.SaveChangesAsync();
        }
    }
}