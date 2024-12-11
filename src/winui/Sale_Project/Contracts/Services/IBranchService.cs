using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Contracts.Services;
public interface IBrandService
{
    // Add a new employee to the system
    Task<Brand> CreateBrand(Brand brand);

    // Mark an employee as unemployed (inactive)
    Task<bool> InactiveBrand(string brandName);

    // Update an existing employee's details
    Task<Brand> UpdateBrand(Brand brand, string newBrandName);

    // Optionally, add a method to list all employees
    Task<IEnumerable<Brand>> GetAllBrands();
}
