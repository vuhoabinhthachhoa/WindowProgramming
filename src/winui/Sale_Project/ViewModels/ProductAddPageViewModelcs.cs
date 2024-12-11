using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Core.Models.Products;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Models;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for adding a new product, handling navigation, data binding, and operations related to product creation.
/// </summary>
public partial class ProductAddViewModel : ObservableRecipient, INavigationAware
{
    private readonly IProductService _productService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IAuthService _authService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;
    private readonly ProductCreationRequestValidator _productCreationRequestValidator;

    /// <summary>
    /// Gets or sets the product creation request data.
    /// </summary>
    [ObservableProperty]
    private ProductCreationRequest _productCreationRequest;

    /// <summary>
    /// Gets or sets the available brands.
    /// </summary>
    [ObservableProperty]
    private string[] _brands;

    /// <summary>
    /// Gets or sets the available categories.
    /// </summary>
    [ObservableProperty]
    private string[] _categories;

    /// <summary>
    /// Gets or sets the image selected for the product.
    /// </summary>
    [ObservableProperty]
    private BitmapImage _pickedImage;

    /// <summary>
    /// Gets or sets the product created after successful addition.
    /// </summary>
    public Product CreatedProduct
    {
        get; set;
    }

    /// <summary>
    /// Gets the available sizes for the product.
    /// </summary>
    public string[] Size { get; set; } = new string[] { "S", "M", "L", "XL", "XXL" };

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductAddViewModel"/> class.
    /// </summary>
    /// <param name="productService">Service for product operations.</param>
    /// <param name="brandService">Service for brand data.</param>
    /// <param name="categoryService">Service for category data.</param>
    /// <param name="navigationService">Service for navigation operations.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    /// <param name="authService">Service for authentication.</param>
    /// <param name="productCreationRequestValidator">Validator for product creation requests.</param>
    public ProductAddViewModel(IProductService productService, IBrandService brandService, ICategoryService categoryService, INavigationService navigationService, IDialogService dialogService, IAuthService authService, ProductCreationRequestValidator productCreationRequestValidator)
    {
        _productService = productService;
        _brandService = brandService;
        _categoryService = categoryService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _authService = authService;
        _productCreationRequestValidator = productCreationRequestValidator;
    }

    /// <summary>
    /// Called when navigated to this ViewModel. Initializes product creation data and fetches brand and category data.
    /// </summary>
    /// <param name="parameter">Navigation parameter.</param>
    public async void OnNavigatedTo(object parameter)
    {
        ProductCreationRequest = new ProductCreationRequest();

        // Fetch brand names and set the Brands property
        var brandNames = await _brandService.GetAllBrands();
        Brands = brandNames?.Select(brand => brand.Name).ToArray();

        // Fetch category names and set the Categories property
        var categoryNames = await _categoryService.GetAllCategories();
        Categories = categoryNames?.Select(category => category.Id).ToArray();

        PickedImage = null;
    }

    /// <summary>
    /// Called when navigated away from this ViewModel. Cleans up data.
    /// </summary>
    public void OnNavigatedFrom()
    {
        ProductCreationRequest = null;
        CreatedProduct = null;
        PickedImage = null;
    }

    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(ProductViewModel).FullName!);
    }

    /// <summary>
    /// Adds a new product using the provided product creation request data.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Allows the user to pick a photo for the product.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
                ProductCreationRequest.File = new StreamContent(Stream.Null);
                PickedImage = null;
            }
        }
        else
        {
            ProductCreationRequest.File = new StreamContent(Stream.Null);
            PickedImage = null;
        }
    }
}
