using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Categories;

namespace Sale_Project.Helpers;
/// <summary>
/// Validates the category creation request.
/// </summary>
public class CategoryCreationRequestValidator
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryCreationRequestValidator"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public CategoryCreationRequestValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Validates the specified category creation request.
    /// </summary>
    /// <param name="categoryCreationRequest">The category creation request.</param>
    /// <returns><c>true</c> if the request is valid; otherwise, <c>false</c>.</returns>
    public bool Validate(CategoryCreationRequest categoryCreationRequest)
    {
        if (string.IsNullOrWhiteSpace(categoryCreationRequest.Id))
        {
            _dialogService.ShowErrorAsync("Error", "Category ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(categoryCreationRequest.Name))
        {
            _dialogService.ShowErrorAsync("Error", "Category name is invalid");
            return false;
        }
        return true;
    }
}
