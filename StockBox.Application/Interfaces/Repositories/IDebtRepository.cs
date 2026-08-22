using Stock_Box.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Repositories
{
    public interface IDebtRepository
    {
        Task<Debt> AddAsync(Debt debt);

        Task<Debt?> GetByIdAsync(int id);

        Task<List<Debt>> GetAllAsync();

        Task<List<Debt>> GetByCustomerIdAsync(int customerId);

        Task UpdateAsync(Debt debt);

        Task DeleteAsync(Debt debt);
    }
}
