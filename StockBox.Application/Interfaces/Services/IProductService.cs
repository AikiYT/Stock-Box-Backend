using StockBox.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task CreateAsync(SaveProductViewModel vm);

        Task<List<ProductViewModel>> GetAllAsync();

        Task<ProductViewModel?> GetByIdAsync(int id);

        Task UpdateAsync(int id, SaveProductViewModel vm);

        Task DeleteAsync(int id);
    }
}
