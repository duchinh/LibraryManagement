using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(CreateCategoryDTO categoryDto);
        Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
    }
}