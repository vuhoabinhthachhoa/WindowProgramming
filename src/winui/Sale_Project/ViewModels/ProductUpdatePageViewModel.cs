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
using Sale_Project.Core.Models.Product;


public partial class ProductUpdateViewModel : ObservableObject, INavigationAware
{
    private readonly IProductService _productService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly ProductValidator productValidator = new();

    [ObservableProperty]
    private Product _currentProduct;

    [ObservableProperty]
    private string _updateAPhotoOutputTextBlock;

    public StreamContent File;

    public ProductUpdateViewModel(IProductService productService, INavigationService navigationService, IDialogService dialogService)
    {
        _productService = productService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        var product = parameter as Product;
        ProductSearchRequest productSearchRequest = new ProductSearchRequest
        {
            Code = product.Code,
            Name = product.Name,
            CategoryName = product.Category.Name,
            BranchName = product.Branch.Name,
            BusinessStatus = product.BusinessStatus
        };

        CurrentProduct = await _productService.GetSelectedProduct(productSearchRequest);
        UpdateAPhotoOutputTextBlock = "No photo selected";
    }

    public void OnNavigatedFrom()
    {
        CurrentProduct = null;
    }

    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(ProductViewModel).FullName!);
    }

    public async Task UpdateProduct()
    {
        // TODO: Allow not updating the file (updating file is required for now)
        if (!productValidator.Validate(CurrentProduct))
        {
            return;
        }
        Product product = await _productService.UpdateProduct(CurrentProduct, File);
        if (product == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Product updated successfully");
        CurrentProduct = product;
    }

    public async Task MarkProductInactive()
    {
        bool confirm = await _dialogService.ShowConfirmAsync("Confirm", "Are you sure you want to mark this product as inactive?");
        if (!confirm)
        {
            return;
        }

        bool result = await _productService.InactiveProduct(CurrentProduct.Id);
        if (!result)
        {
            await _dialogService.ShowErrorAsync("Error", "Failed to mark product as unemployed.");
            return;
        }

        CurrentProduct.BusinessStatus = false;
        await _dialogService.ShowSuccessAsync("Success", "Product marked as unemployed successfully.");
    }

    public async Task MarkProductActive()
    {
        bool confirm = await _dialogService.ShowConfirmAsync("Confirm", "Are you sure you want to mark this product as active?");
        if (!confirm)
        {
            return;
        }

        bool result = await _productService.ActiveProduct(CurrentProduct.Id);
        if (!result)
        {
            await _dialogService.ShowErrorAsync("Error", "Failed to mark product as unemployed.");
            return;
        }

        CurrentProduct.BusinessStatus = true;
        await _dialogService.ShowSuccessAsync("Success", "Product marked as unemployed successfully.");
    }

    public async Task UpdateAPhoto()
    {
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        var file = await openPicker.PickSingleFileAsync();

        if (file != null)
        {
            UpdateAPhotoOutputTextBlock = "Picked photo: " + file.Name;

            try
            {
                var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                //UpdateAPhotoOutputTextBlock.Text += "\nStream type: " + fileStream.GetType().FullName;

                // Explicitly convert IRandomAccessStream to Stream
                File = new StreamContent(fileStream.AsStreamForRead());
            }
            catch (Exception ex)
            {
                UpdateAPhotoOutputTextBlock = "Error opening file: " + ex.Message;
                File = new StreamContent(Stream.Null);
            }
        }
        else
        {
            UpdateAPhotoOutputTextBlock = "Operation cancelled.";
            File = new StreamContent(Stream.Null);
        }
    }
} 
