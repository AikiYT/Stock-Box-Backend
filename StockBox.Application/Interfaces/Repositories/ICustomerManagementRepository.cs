using Stock_Box.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Repositories
{
    public interface ICustomerManagementRepository
    {
        Task<Customer> CreateCompleteCustomerAsync(
            Customer customer,
            CustomerServiceRecord? serviceRecord,
            Debt? debt);

        Task<Customer?> GetCompleteCustomerByIdAsync(int id);

        Task<List<Customer>> GetAllCompleteCustomersAsync();
    }
}
