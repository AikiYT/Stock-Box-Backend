using StockBox.Application.ViewModels.ServiceRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Services
{
    public interface ICustomerServiceRecordService
    {
        Task CreateAsync(SaveCustomerServiceRecordViewModel vm);

        Task<List<CustomerServiceRecordViewModel>> GetAllAsync();

        Task<CustomerServiceRecordViewModel?> GetByIdAsync(int id);

        Task<List<CustomerServiceRecordViewModel>> GetByCustomerIdAsync(int customerId);

        Task UpdateAsync(int id, SaveCustomerServiceRecordViewModel vm);

        Task DeleteAsync(int id);
    }
}