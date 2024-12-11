using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.Helpers;
public class ProductCreationRequestValidator
{

    private readonly IDialogService _dialogService;
    public ProductCreationRequestValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    public bool Validate(ProductCreationRequest productCreationRequest)
    {
        if (string.IsNullOrWhiteSpace(productCreationRequest.Data.Name))
        {
            _dialogService.ShowErrorAsync("Error", "Product name is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(productCreationRequest.Data.CategoryId))
        {
            _dialogService.ShowErrorAsync("Error", "Product Category ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(productCreationRequest.Data.BrandName))
        {
            _dialogService.ShowErrorAsync("Error", "Product Brand name is invalid");
            return false;
        }
        if (!IsValidNumericValue(productCreationRequest.Data.ImportPrice))
        {
            _dialogService.ShowErrorAsync("Error", "Import price is invalid");
            return false;
        }
        if (!IsValidNumericValue(productCreationRequest.Data.SellingPrice))
        {
            _dialogService.ShowErrorAsync("Error", "Selling price is invalid");
            return false;
        }
        if (!IsValidNumericValue(productCreationRequest.Data.InventoryQuantity))
        {
            _dialogService.ShowErrorAsync("Error", "Inventory quantity is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(productCreationRequest.Data.Size))
        {
            _dialogService.ShowErrorAsync("Error", "Product Size is invalid");
            return false;
        }
        if (!IsValidNumericValue(productCreationRequest.Data.DiscountPercent))
        {
            _dialogService.ShowErrorAsync("Error", "Discount percent is invalid");
            return false;
        }
        return true;
    }

    // Helper method to validate numeric value
    public bool IsValidNumericValue(double? value)
    {
        return value.HasValue &&
               !double.IsNaN(value.Value) &&
               !double.IsInfinity(value.Value) &&
               value.Value >= 0;
    }
}
