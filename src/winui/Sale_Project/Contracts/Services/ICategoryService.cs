using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models.Categories;

namespace Sale_Project.Contracts.Services;
public interface ICategoryService
{
    // Add a new category to the system
    Task<Category> CreateCategory(CategoryCreationRequest categoryCreationRequest);

    // Mark an category as inactive
    Task<bool> InactiveCategory(string categoryName);

    // Update an existing category's details
    Task<Category> UpdateCategory(Category category, string newCategoryName);

    // Optionally, add a method to list all categories
    Task<IEnumerable<Category>> GetAllCategories();

    Task<Category> GetCategoryById(string categoryId);
}
