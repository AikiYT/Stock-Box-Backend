using StockBox.Application.ViewModels.Deudas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Services
{
    public interface IDebtService
    {
        Task CreateAsync(SaveDebtViewModel vm);

        Task<List<DebtViewModel>> GetAllAsync();

        Task<DebtViewModel?> GetByIdAsync(int id);

        Task<List<DebtViewModel>> GetByCustomerIdAsync(int customerId);

        Task UpdateAsync(int id, SaveDebtViewModel vm);

        Task DeleteAsync(int id);
    }
}
