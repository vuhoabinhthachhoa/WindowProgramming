using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Brands;

namespace Sale_Project.Helpers;
/// <summary>
/// Validates the brand creation request.
/// </summary>
public class BrandCreationRequestValidator
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrandCreationRequestValidator"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public BrandCreationRequestValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Validates the specified brand creation request.
    /// </summary>
    /// <param name="brandCreationRequest">The brand creation request.</param>
    /// <returns><c>true</c> if the request is valid; otherwise, <c>false</c>.</returns>
    public bool Validate(BrandCreationRequest brandCreationRequest)
    {
        if (!IsValidNumericValue(brandCreationRequest.Id))
        {
            _dialogService.ShowErrorAsync("Error", "Brand ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(brandCreationRequest.Name))
        {
            _dialogService.ShowErrorAsync("Error", "Brand name is invalid");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Determines whether the specified value is a valid numeric value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
    public bool IsValidNumericValue(int? value)
    {
        return value.HasValue &&
               value.Value >= 0;
    }
}
