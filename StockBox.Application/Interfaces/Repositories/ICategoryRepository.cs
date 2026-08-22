using Stock_Box.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> AddAsync(Category category);

        Task<Category?> GetByIdAsync(int id);

        Task<List<Category>> GetAllAsync();

        Task UpdateAsync(Category category);

        Task DeleteAsync(Category category);
    }
}
