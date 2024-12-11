using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models.Product;
using Sale_Project.ViewModels;
using Windows.Storage.Pickers;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the ProductAddPage, providing UI logic for adding a new product.
/// </summary>
public sealed partial class ProductAddPage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing data.
    /// </summary>
    public ProductAddViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the ProductAddPage, setting up the ViewModel and DataContext.
    /// </summary>
    public ProductAddPage()
    {
        ViewModel = App.GetService<ProductAddViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    /// <summary>
    /// Handles the GoBack button click event to navigate back to the previous page.
    /// </summary>
    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoBack();
    }

    /// <summary>
    /// Handles the Add button click event to add a new product using the ViewModel.
    /// </summary>
    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.AddProduct();
    }

    /// <summary>
    /// Handles the PickAPhoto button click event to select a photo for the product.
    /// </summary>
    public async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.PickAPhoto();
    }
}
