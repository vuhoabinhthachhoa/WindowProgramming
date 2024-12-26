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
public class CategoryValidator
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryValidator"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public CategoryValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Validates the specified category creation request.
    /// </summary>
    /// <param name="category">The category creation request.</param>
    /// <returns><c>true</c> if the request is valid; otherwise, <c>false</c>.</returns>
    public bool Validate(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Id))
        {
            _dialogService.ShowErrorAsync("Error", "Category ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            _dialogService.ShowErrorAsync("Error", "Category name is invalid");
            return false;
        }
        return true;
    }
}
