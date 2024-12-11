using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.Helpers;
public class ProductValidator
{
    private readonly IDialogService _dialogService;
    public ProductValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    public bool Validate(Product product)
    {
        if (!IsValidNumericValue(product.Id))
        {
            _dialogService.ShowErrorAsync("Error", "Product ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            _dialogService.ShowErrorAsync("Error", "Product name is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(product.Code))
        {
            _dialogService.ShowErrorAsync("Error", "Product Code is invalid");
            return false;
        }
        if (!IsValidNumericValue(product.ImportPrice))
        {
            _dialogService.ShowErrorAsync("Error", "Product Import price is invalid");
            return false;
        }
        if (!IsValidNumericValue(product.SellingPrice))
        {
            _dialogService.ShowErrorAsync("Error", "Product Selling price is invalid");
            return false;
        }
        if (!IsValidNumericValue(product.InventoryQuantity))
        {
            _dialogService.ShowErrorAsync("Error", "Product Inventory quantity is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(product.Size))
        {
            _dialogService.ShowErrorAsync("Error", "Product Size is invalid");
            return false;
        }
        if (!IsValidNumericValue(product.DiscountPercent))
        {
            _dialogService.ShowErrorAsync("Error", "Product Discount is invalid");
            return false;
        }
        return true;
    }

    // Helper method to validate salary
    public bool IsValidNumericValue(double? value)
    {
        return value.HasValue &&
               !double.IsNaN(value.Value) &&
               !double.IsInfinity(value.Value) &&
               value.Value >= 0;
    }
}
