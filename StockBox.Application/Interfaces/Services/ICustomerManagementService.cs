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
        Task<CustomerManagementViewModel> CreateAsync(
            CreateCustomerManagementViewModel vm);

        Task<CustomerManagementViewModel?> GetByIdAsync(int id);

        Task<List<CustomerManagementViewModel>> GetAllAsync();
    }
}