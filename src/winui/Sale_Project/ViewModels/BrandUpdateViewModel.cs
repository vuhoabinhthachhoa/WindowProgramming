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
using Sale_Project.Core.Models.Brands;
using Microsoft.UI.Xaml.Media.Imaging;

/// <summary>
/// ViewModel for updating a brand, including navigation handling and operations
/// like updating, marking active/inactive, and updating photos.
/// </summary>
public partial class BrandUpdateViewModel : ObservableObject, INavigationAware
{
    private readonly IBrandService _brandService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly BrandValidator _brandValidator;

    /// <summary>
    /// Current brand being updated.
    /// </summary>
    [ObservableProperty]
    private Brand _currentBrand;

    [ObservableProperty]
    private string _oldBrandName;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrandUpdateViewModel"/> class.
    /// </summary>
    /// <param name="brandService">Service for brand operations.</param>
    /// <param name="navigationService">Service for navigation handling.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    /// <param name="brandValidator">Validator for brand data.</param>
    public BrandUpdateViewModel(IBrandService brandService, INavigationService navigationService, IDialogService dialogService, BrandValidator brandValidator)
    {
        _brandService = brandService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _brandValidator = brandValidator;
    }

    /// <summary>
    /// Handles navigation to this ViewModel.
    /// </summary>
    /// <param name="parameter">Navigation parameter containing brand information.</param>
    public void OnNavigatedTo(object parameter)
    {
        var brand = parameter as Brand;
        CurrentBrand = brand;
        OldBrandName = brand.Name;
    }

    /// <summary>
    /// Handles navigation away from this ViewModel.
    /// </summary>
    public void OnNavigatedFrom()
    {
        CurrentBrand = null;
    }

    /// <summary>
    /// Navigates back to the brand list view.
    /// </summary>
    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(BrandViewModel).FullName!);
    }

    /// <summary>
    /// Updates the current brand with the latest data.
    /// </summary>
    public async Task UpdateBrand()
    {
        if (!_brandValidator.Validate(CurrentBrand))
        {
            return;
        }

        Brand brand = await _brandService.UpdateBrand(CurrentBrand, OldBrandName);
        if (brand == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Brand updated successfully");
        CurrentBrand = brand;
    }

    /// <summary>
    /// Marks the current brand as inactive.
    /// </summary>
    public async Task MarkBrandInactive()
    {
        bool confirm = await _dialogService.ShowConfirmAsync("Confirm", "Are you sure you want to mark this brand as inactive?");
        if (!confirm)
        {
            return;
        }

        bool result = await _brandService.InactiveBrand(CurrentBrand.Name);
        if (!result)
        {
            await _dialogService.ShowErrorAsync("Error", "Failed to mark brand as inactive.");
            return;
        }

        CurrentBrand.BusinessStatus = false;
        await _dialogService.ShowSuccessAsync("Success", "Brand marked as inactive successfully.");
    }
}
