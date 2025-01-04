using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models.Brands;

namespace Sale_Project.Contracts.Services;
public interface IBrandService
{
    // Add a new brand to the system
    Task<Brand> CreateBrand(BrandCreationRequest brandCreationRequest);

    // Mark an brand as inactive
    Task<bool> InactiveBrand(string brandName);

    // Update an existing brand's details
    Task<Brand> UpdateBrand(Brand brand, string oldBrandName);

    // Optionally, add a method to list all brands
    Task<IEnumerable<Brand>> GetAllBrands();
}
