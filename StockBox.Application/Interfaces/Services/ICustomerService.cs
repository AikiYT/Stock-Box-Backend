using StockBox.Application.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task CreateAsync(SaveCustomerViewModel vm);

        Task<List<CustomerViewModel>> GetAllAsync();

        Task<CustomerViewModel?> GetByIdAsync(int id);

        Task UpdateAsync(int id, SaveCustomerViewModel vm);

        Task DeleteAsync(int id);
    }
}
