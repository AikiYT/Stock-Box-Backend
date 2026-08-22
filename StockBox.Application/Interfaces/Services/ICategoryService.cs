using StockBox.Application.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task CreateAsync(SaveCategoryViewModel vm);

        Task<List<CategoryViewModel>> GetAllAsync();

        Task<CategoryViewModel?> GetByIdAsync(int id);

        Task UpdateAsync(int id, SaveCategoryViewModel vm);

        Task DeleteAsync(int id);
    }
}
