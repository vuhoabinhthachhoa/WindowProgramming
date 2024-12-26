using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Contracts.ViewModels;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Imaging;
using Sale_Project.Core.Models.Categories;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for adding a new category, handling navigation, data binding, and operations related to category creation.
/// </summary>
public partial class CategoryAddViewModel : ObservableRecipient, INavigationAware
{
    private readonly ICategoryService _categoryService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IAuthService _authService;
    private readonly IBrandService _brandService;
    private readonly CategoryCreationRequestValidator _categoryCreationRequestValidator;

    /// <summary>
    /// Gets or sets the category creation request data.
    /// </summary>
    [ObservableProperty]
    private CategoryCreationRequest _categoryCreationRequest;

    /// <summary>
    /// Gets or sets the category created after successful addition.
    /// </summary>
    public Category CreatedCategory
    {
        get; set;
    }

    /// <summary>
    /// Gets the available sizes for the category.
    /// </summary>
    public string[] Size { get; set; } = new string[] { "S", "M", "L", "XL", "XXL" };

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryAddViewModel"/> class.
    /// </summary>
    /// <param name="categoryService">Service for category operations.</param>
    /// <param name="navigationService">Service for navigation operations.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    /// <param name="authService">Service for authentication.</param>
    /// <param name="categoryCreationRequestValidator">Validator for category creation requests.</param>
    public CategoryAddViewModel(ICategoryService categoryService, INavigationService navigationService, IDialogService dialogService, IAuthService authService, CategoryCreationRequestValidator categoryCreationRequestValidator)
    {
        _categoryService = categoryService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _authService = authService;
        _categoryCreationRequestValidator = categoryCreationRequestValidator;
    }

    /// <summary>
    /// Called when navigated to this ViewModel. Initializes category creation data and fetches brand and category data.
    /// </summary>
    /// <param name="parameter">Navigation parameter.</param>
    public async void OnNavigatedTo(object parameter)
    {
        CategoryCreationRequest = new CategoryCreationRequest();
    }

    /// <summary>
    /// Called when navigated away from this ViewModel. Cleans up data.
    /// </summary>
    public void OnNavigatedFrom()
    {
        CategoryCreationRequest = null;
        CreatedCategory = null;
    }

    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(CategoryViewModel).FullName!);
    }

    /// <summary>
    /// Adds a new category using the provided category creation request data.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddCategory()
    {
        if (CreatedCategory != null)
        {
            await _dialogService.ShowErrorAsync("Error", "Category has been already added!");
            return;
        }
        if (!_categoryCreationRequestValidator.Validate(CategoryCreationRequest))
        {
            return;
        }
        Category category = await _categoryService.CreateCategory(CategoryCreationRequest);
        if (category == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Category added successfully");
        CreatedCategory = category;
        GoBack();
    }
}
