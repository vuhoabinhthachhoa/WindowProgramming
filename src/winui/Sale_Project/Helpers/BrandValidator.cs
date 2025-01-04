﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Brands;

namespace Sale_Project.Helpers;
/// <summary>
/// Validates the brand creation request.
/// </summary>
public class BrandValidator
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrandValidator"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public BrandValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Validates the specified brand creation request.
    /// </summary>
    /// <param name="brand">The brand creation request.</param>
    /// <returns><c>true</c> if the request is valid; otherwise, <c>false</c>.</returns>
    public bool Validate(Brand brand)
    {
        if (!IsValidNumericValue(brand.Id))
        {
            _dialogService.ShowErrorAsync("Error", "Brand ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(brand.Name))
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
