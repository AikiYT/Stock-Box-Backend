using Stock_Box.Entities;
using StockBox.Application.ViewModels.ServiceRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Repositories
{
    public interface ICustomerServiceRecordRepository
    {
        Task<CustomerServiceRecord> AddAsync(CustomerServiceRecord record);

        Task<CustomerServiceRecord?> GetByIdAsync(int id);

        Task<List<CustomerServiceRecord>> GetAllAsync();

        Task<List<CustomerServiceRecord>> GetByCustomerIdAsync(int customerId);

        Task UpdateAsync(CustomerServiceRecord record);

        Task DeleteAsync(CustomerServiceRecord record);
    }
}