using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Contracts.Services;
public interface ICategoryService
{
    // Add a new employee to the system
    Task<Category> CreateCategory(Category category);

    // Mark an employee as unemployed (inactive)
    Task<bool> InactiveCategory(string categoryName);

    // Update an existing employee's details
    Task<Category> UpdateCategory(Category category, string newCategoryName);

    // Optionally, add a method to list all employees
    Task<IEnumerable<Category>> GetAllCategories();

    Task<Category> GetCategoryById(string categoryId);
}
