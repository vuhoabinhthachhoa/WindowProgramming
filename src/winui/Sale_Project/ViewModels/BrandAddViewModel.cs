using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Contracts.ViewModels;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Imaging;
using Sale_Project.Core.Models.Brands;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for adding a new brand, handling navigation, data binding, and operations related to brand creation.
/// </summary>
public partial class BrandAddViewModel : ObservableRecipient, INavigationAware
{
    private readonly IBrandService _brandService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IAuthService _authService;
    private readonly BrandCreationRequestValidator _brandCreationRequestValidator;

    /// <summary>
    /// Gets or sets the brand creation request data.
    /// </summary>
    [ObservableProperty]
    private BrandCreationRequest _brandCreationRequest;

    /// <summary>
    /// Gets or sets the brand created after successful addition.
    /// </summary>
    public Brand CreatedBrand
    {
        get; set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrandAddViewModel"/> class.
    /// </summary>
    /// <param name="brandService">Service for brand operations.</param>
    /// <param name="navigationService">Service for navigation operations.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    /// <param name="authService">Service for authentication.</param>
    /// <param name="brandCreationRequestValidator">Validator for brand creation requests.</param>
    public BrandAddViewModel(IBrandService brandService, INavigationService navigationService, IDialogService dialogService, IAuthService authService, BrandCreationRequestValidator brandCreationRequestValidator)
    {
        _brandService = brandService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _authService = authService;
        _brandCreationRequestValidator = brandCreationRequestValidator;
    }

    /// <summary>
    /// Called when navigated to this ViewModel. Initializes brand creation data and fetches brand and brand data.
    /// </summary>
    /// <param name="parameter">Navigation parameter.</param>
    public async void OnNavigatedTo(object parameter)
    {
        BrandCreationRequest = new BrandCreationRequest();
    }

    /// <summary>
    /// Called when navigated away from this ViewModel. Cleans up data.
    /// </summary>
    public void OnNavigatedFrom()
    {
        BrandCreationRequest = null;
        CreatedBrand = null;
    }

    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(BrandViewModel).FullName!);
    }

    /// <summary>
    /// Adds a new brand using the provided brand creation request data.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddBrand()
    {
        if (CreatedBrand != null)
        {
            await _dialogService.ShowErrorAsync("Error", "Brand has been already added!");
            return;
        }
        if (!_brandCreationRequestValidator.Validate(BrandCreationRequest))
        {
            return;
        }
        Brand brand = await _brandService.CreateBrand(BrandCreationRequest);
        if (brand == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Brand added successfully");
        CreatedBrand = brand;
        GoBack();
    }
}
