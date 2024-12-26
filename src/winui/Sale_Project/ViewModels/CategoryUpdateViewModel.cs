using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Contracts.Services;
using Sale_Project.Services;
using Sale_Project.ViewModels;
using Sale_Project.Helpers;
using Microsoft.UI.Xaml.Navigation;
using Sale_Project;
using Windows.Storage.Pickers;
using Sale_Project.Core.Models.Categories;
using Microsoft.UI.Xaml.Media.Imaging;

/// <summary>
/// ViewModel for updating a category, including navigation handling and operations
/// like updating, marking active/inactive, and updating photos.
/// </summary>
public partial class CategoryUpdateViewModel : ObservableObject, INavigationAware
{
    private readonly ICategoryService _categoryService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly CategoryValidator _categoryValidator;

    /// <summary>
    /// Current category being updated.
    /// </summary>
    [ObservableProperty]
    private Category _currentCategory;


    [ObservableProperty]
    private string _newCategoryName;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryUpdateViewModel"/> class.
    /// </summary>
    /// <param name="categoryService">Service for category operations.</param>
    /// <param name="navigationService">Service for navigation handling.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    /// <param name="categoryValidator">Validator for category data.</param>
    public CategoryUpdateViewModel(ICategoryService categoryService, INavigationService navigationService, IDialogService dialogService, CategoryValidator categoryValidator)
    {
        _categoryService = categoryService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _categoryValidator = categoryValidator;
    }

    /// <summary>
    /// Handles navigation to this ViewModel.
    /// </summary>
    /// <param name="parameter">Navigation parameter containing category information.</param>
    public async void OnNavigatedTo(object parameter)
    {
        var category = parameter as Category;
        CurrentCategory = category;
    }

    /// <summary>
    /// Handles navigation away from this ViewModel.
    /// </summary>
    public void OnNavigatedFrom()
    {
        CurrentCategory = null;
    }

    /// <summary>
    /// Navigates back to the category list view.
    /// </summary>
    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(CategoryViewModel).FullName!);
    }

    /// <summary>
    /// Updates the current category with the latest data.
    /// </summary>
    public async Task UpdateCategory()
    {
        if (!_categoryValidator.Validate(CurrentCategory))
        {
            return;
        }

        Category category = await _categoryService.UpdateCategory(CurrentCategory, CurrentCategory.Name);
        if (category == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Category updated successfully");
        CurrentCategory = category;
    }

    /// <summary>
    /// Marks the current category as inactive.
    /// </summary>
    public async Task MarkCategoryInactive()
    {
        bool confirm = await _dialogService.ShowConfirmAsync("Confirm", "Are you sure you want to mark this category as inactive?");
        if (!confirm)
        {
            return;
        }

        bool result = await _categoryService.InactiveCategory(CurrentCategory.Id);
        if (!result)
        {
            await _dialogService.ShowErrorAsync("Error", "Failed to mark category as inactive.");
            return;
        }

        CurrentCategory.BusinessStatus = false;
        await _dialogService.ShowSuccessAsync("Success", "Category marked as inactive successfully.");
    }
}
