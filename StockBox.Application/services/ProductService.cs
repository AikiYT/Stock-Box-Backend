using Stock_Box.Entities;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductViewModel> CreateAsync(SaveProductViewModel vm)
        {
            var product = new Product
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                Stock = vm.Stock,
                MinimumStock = vm.MinimumStock,
                CategoryId = vm.CategoryId
            };

            await _productRepository.AddAsync(product);

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                MinimumStock = product.MinimumStock,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty
            };
        }

        public async Task<List<ProductViewModel>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                MinimumStock = p.MinimumStock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty
            }).ToList();
        }

        public async Task<ProductViewModel?> GetByIdAsync(int id)
        {
            var p = await _productRepository.GetByIdAsync(id);

            if (p == null)
                return null;

            return new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                MinimumStock = p.MinimumStock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty
            };
        }

        public async Task UpdateAsync(int id, SaveProductViewModel vm)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            product.Name = vm.Name;
            product.Description = vm.Description;
            product.Price = vm.Price;
            product.Stock = vm.Stock;
            product.MinimumStock = vm.MinimumStock;
            product.CategoryId = vm.CategoryId;

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            await _productRepository.DeleteAsync(product);
        }
    }
}