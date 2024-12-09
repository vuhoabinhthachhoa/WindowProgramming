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
using Microsoft.UI.Xaml.Media.Imaging;


public partial class ProductUpdateViewModel : ObservableObject, INavigationAware
{
    private readonly IProductService _productService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly ProductValidator _productValidator;

    [ObservableProperty]
    private Product _currentProduct;

    //[ObservableProperty]
    //private string _updateAPhotoOutputTextBlock;

    [ObservableProperty]
    private BitmapImage _pickedImage;

    public StreamContent File;

    public ProductUpdateViewModel(IProductService productService, INavigationService navigationService, IDialogService dialogService, ProductValidator productValidator)
    {
        _productService = productService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _productValidator = productValidator;
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
        //UpdateAPhotoOutputTextBlock = "";
        PickedImage = null;
    }

    public void OnNavigatedFrom()
    {
        CurrentProduct = null;
        PickedImage = null;
    }

    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(ProductViewModel).FullName!);
    }

    public async Task UpdateProduct()
    {
        if (!_productValidator.Validate(CurrentProduct))
        {
            return;
        }

        if (File == null)
        {
            try
            {
                var httpClient = new HttpClient();
                var imageStream = await httpClient.GetStreamAsync(CurrentProduct.ImageUrl);
                File = new StreamContent(imageStream);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync("Error", "Failed to load image from URL: " + ex.Message);
                return;
            }
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
            //UpdateAPhotoOutputTextBlock = "Picked photo: " + file.Name;

            try
            {
                var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                //UpdateAPhotoOutputTextBlock.Text += "\nStream type: " + fileStream.GetType().FullName;

                // Explicitly convert IRandomAccessStream to Stream
                File = new StreamContent(fileStream.AsStreamForRead());

                // Create a BitmapImage from the file stream
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                CurrentProduct.ImageUrl = file.Path;
                PickedImage = bitmapImage;
            }
            catch (Exception ex)
            {
                //UpdateAPhotoOutputTextBlock = "Error opening file: " + ex.Message;
                File = new StreamContent(Stream.Null);
                PickedImage = null;

            }
        }
        else
        {
            //UpdateAPhotoOutputTextBlock = "Operation cancelled.";
            File = new StreamContent(Stream.Null);
            PickedImage = null;

        }
    }
} 
