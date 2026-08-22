using Stock_Box.Entities;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(SaveCategoryViewModel vm)
        {
            var category = new Category
            {
                Name = vm.Name,
                Description = vm.Description
            };

            await _categoryRepository.AddAsync(category);
        }

        public async Task<List<CategoryViewModel>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<CategoryViewModel?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task UpdateAsync(int id, SaveCategoryViewModel vm)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            category.Name = vm.Name;
            category.Description = vm.Description;

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            await _categoryRepository.DeleteAsync(category);
        }
    }
}