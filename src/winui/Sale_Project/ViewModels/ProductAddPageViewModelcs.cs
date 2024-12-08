﻿using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Core.Models.Product;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Models;
using Windows.Storage.Pickers;

namespace Sale_Project.ViewModels;

public partial class ProductAddViewModel : ObservableRecipient, INavigationAware
{
    private readonly IProductService _productService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IAuthService _authService;
    private readonly ProductCreationRequestValidator _productCreationRequestValidator;

    [ObservableProperty]
    private ProductCreationRequest _productCreationRequest;

    [ObservableProperty]
    private string _pickAPhotoOutputTextBlock;

    //[ObservableProperty]
    //private RegistrationRequest _registrationRequest;

    public Product CreatedProduct
    {
        get; set;
    }


    public ProductAddViewModel(IProductService productService, INavigationService navigationService, IDialogService dialogService, IAuthService authService, ProductCreationRequestValidator productCreationRequestValidator)
    {
        _productService = productService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _authService = authService;
        _productCreationRequestValidator = productCreationRequestValidator;
    }

    public void OnNavigatedTo(object parameter)
    {
        ProductCreationRequest = new ProductCreationRequest();
        //RegistrationRequest = new RegistrationRequest();
        PickAPhotoOutputTextBlock = "";
    }

    public void OnNavigatedFrom()
    {
        ProductCreationRequest = null;
        //RegistrationRequest = null;
        CreatedProduct = null;
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
    }

    public async Task PickAPhoto()
    {
        //PickAPhotoOutputTextBlock.Text = "";

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
            PickAPhotoOutputTextBlock = "Picked photo: " + file.Name;

            try
            {
                var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                //PickAPhotoOutputTextBlock.Text += "\nStream type: " + fileStream.GetType().FullName;

                // Explicitly convert IRandomAccessStream to Stream
                ProductCreationRequest.File = new StreamContent(fileStream.AsStreamForRead());
            }
            catch (Exception ex)
            {
                PickAPhotoOutputTextBlock = "Error opening file: " + ex.Message;
                ProductCreationRequest.File = new StreamContent(Stream.Null);
            }
        }
        else
        {
            PickAPhotoOutputTextBlock = "Operation cancelled.";
            ProductCreationRequest.File = new StreamContent(Stream.Null);
        }
    }
}

