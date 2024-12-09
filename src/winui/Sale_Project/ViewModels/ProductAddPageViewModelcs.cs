using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Core.Models.Product;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Models;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Sale_Project.ViewModels;

public partial class ProductAddViewModel : ObservableRecipient, INavigationAware
{
    private readonly IProductService _productService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IAuthService _authService;
    private readonly IBranchService _branchService;
    private readonly ICategoryService _categoryService;
    private readonly ProductCreationRequestValidator _productCreationRequestValidator;

    [ObservableProperty]
    private ProductCreationRequest _productCreationRequest;

    //[ObservableProperty]
    //private string _pickAPhotoOutputTextBlock;

    [ObservableProperty]
    private string[] _branches;

    [ObservableProperty]
    private string[] _categories;

    [ObservableProperty]
    private BitmapImage _pickedImage;
    //[ObservableProperty]
    //private RegistrationRequest _registrationRequest;

    public Product CreatedProduct
    {
        get; set;
    }


    public string[] Size { get; set; } = new string[] { "S", "M", "L", "XL", "XXL" };

    public ProductAddViewModel(IProductService productService, IBranchService branchService, ICategoryService categoryService, INavigationService navigationService, IDialogService dialogService, IAuthService authService, ProductCreationRequestValidator productCreationRequestValidator)
    {
        _productService = productService;
        _branchService = branchService;
        _categoryService = categoryService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _authService = authService;
        _productCreationRequestValidator = productCreationRequestValidator;
    }

    public async void OnNavigatedTo(object parameter)
    {
        ProductCreationRequest = new ProductCreationRequest();
        //PickAPhotoOutputTextBlock = "";

        // Fetch branch names and set the Branches property
        var branchNames = await _branchService.GetAllBranches();
        Branches = branchNames?.Select(branch => branch.Name).ToArray();

        // Fetch category names and set the Categories property
        var categoryNames = await _categoryService.GetAllCategories();
        Categories = categoryNames?.Select(category => category.Id).ToArray();

        PickedImage = null;
    }

    public void OnNavigatedFrom()
    {
        ProductCreationRequest = null;
        //RegistrationRequest = null;
        CreatedProduct = null;
        PickedImage = null;

    }

    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(ProductViewModel).FullName!);
    }

  

    public async Task AddProduct()
    {
        if (CreatedProduct != null)
        {
            await _dialogService.ShowErrorAsync("Error", "Product has been already added!");
            return;
        }
        if (!_productCreationRequestValidator.Validate(ProductCreationRequest))
        {
            return;
        }
        Product product = await _productService.AddProduct(ProductCreationRequest);
        if (product == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Product added successfully");
        CreatedProduct = product;
        GoBack();
    }

    public async Task PickAPhoto()
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
            //PickAPhotoOutputTextBlock = "Picked photo: " + file.Name;

            try
            {
                var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                ProductCreationRequest.File = new StreamContent(fileStream.AsStreamForRead());

                // Create a BitmapImage from the file stream
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                PickedImage = bitmapImage;
            }
            catch (Exception ex)
            {
                //PickAPhotoOutputTextBlock = "Error opening file: " + ex.Message;
                ProductCreationRequest.File = new StreamContent(Stream.Null);
                PickedImage = null;
            }
        }
        else
        {
            //PickAPhotoOutputTextBlock = "Operation cancelled.";
            ProductCreationRequest.File = new StreamContent(Stream.Null);
            PickedImage = null;
        }
    }
}

