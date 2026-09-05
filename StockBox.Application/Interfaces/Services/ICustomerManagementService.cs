using StockBox.Application.ViewModels.CustomerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Services
{
    public interface ICustomerManagementService
    {
        Task<List<CustomerManagementViewModel>> GetAllAsync();

        Task<CustomerManagementViewModel?> GetByIdAsync(int id);

        Task<CustomerManagementViewModel> CreateAsync(
            CreateCustomerManagementViewModel vm);

        Task UpdateCustomerAsync(
            int id,
            CreateCustomerManagementViewModel vm);

        Task PayDebtAsync(
            int customerId,
            int debtId,
            PaymentViewModel vm);

        Task DeleteAsync(int id);
    }
}