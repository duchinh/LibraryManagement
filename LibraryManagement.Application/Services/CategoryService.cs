using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new InvalidOperationException("Không tìm thấy danh mục");
            return category;
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDTO categoryDto)
        {
            if (await _unitOfWork.Categories.ExistsByNameAsync(categoryDto.Name))
            {
                throw new ArgumentException("Category name already exists");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var addedCategory = await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return addedCategory;
        }

        public async Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                throw new ArgumentException("Category not found");
            }

            if (!string.IsNullOrEmpty(categoryDto.Name) && categoryDto.Name != category.Name)
            {
                if (await _unitOfWork.Categories.ExistsByNameAsync(categoryDto.Name))
                {
                    throw new ArgumentException("Category name already exists");
                }
                category.Name = categoryDto.Name;
            }

            if (!string.IsNullOrEmpty(categoryDto.Description))
            {
                category.Description = categoryDto.Description;
            }

            category.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                throw new ArgumentException("Category not found");
            }

            var books = await _bookRepository.GetByCategoryIdAsync(id);
            if (books.Any())
            {
                throw new InvalidOperationException("Cannot delete category with existing books");
            }

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsCategoryExistsAsync(string name)
        {
            return await _unitOfWork.Categories.ExistsByNameAsync(name);
        }

        public async Task<int> GetBookCountByCategoryAsync(int categoryId)
        {
            var books = await _unitOfWork.Categories.GetBooksByCategoryIdAsync(categoryId);
            return books.Count();
        }

        public async Task<IEnumerable<Category>> GetPopularCategoriesAsync(int count)
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories
                .OrderByDescending(c => c.Books.Count)
                .Take(count);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            if (!await _unitOfWork.Categories.ExistsAsync(categoryId))
            {
                throw new Exception("Danh mục không tồn tại");
            }

            return await _bookRepository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _unitOfWork.Categories.ExistsAsync(id);
        }

        public async Task<bool> CategoryExistsByNameAsync(string name)
        {
            return await _unitOfWork.Categories.ExistsByNameAsync(name);
        }
    }
}