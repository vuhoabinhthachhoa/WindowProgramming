using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.Helpers;
/// <summary>
/// Provides validation methods for Product objects.
/// </summary>
public class ProductValidator
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductValidator"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service to show error messages.</param>
    public ProductValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Validates the specified product.
    /// </summary>
    /// <param name="product">The product to validate.</param>
    /// <returns><c>true</c> if the product is valid; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Determines whether the specified value is a valid numeric value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
    public bool IsValidNumericValue(double? value)
    {
        return value.HasValue &&
               !double.IsNaN(value.Value) &&
               !double.IsInfinity(value.Value) &&
               value.Value >= 0;
    }
}

