using Stock_Box.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);

        Task<Customer?> GetByIdAsync(int id);

        Task<List<Customer>> GetAllAsync();

        Task UpdateAsync(Customer customer);

        Task DeleteAsync(Customer customer);
    }
}
