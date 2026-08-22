using Stock_Box.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);

        Task<Product?> GetByIdAsync(int id);

        Task<List<Product>> GetAllAsync();

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}
