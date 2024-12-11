using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.Helpers;
/// <summary>
/// Validates the product creation request.
/// </summary>
public class ProductCreationRequestValidator
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCreationRequestValidator"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public ProductCreationRequestValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Validates the specified product creation request.
    /// </summary>
    /// <param name="productCreationRequest">The product creation request.</param>
    /// <returns><c>true</c> if the request is valid; otherwise, <c>false</c>.</returns>
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
