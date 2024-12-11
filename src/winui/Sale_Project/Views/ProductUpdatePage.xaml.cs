using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.ViewModels;
using Windows.Storage.Pickers;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the ProductUpdatePage, providing UI logic for updating and managing product details.
/// </summary>
public sealed partial class ProductUpdatePage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing product update operations.
    /// </summary>
    public ProductUpdateViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the ProductUpdatePage, setting up the ViewModel and DataContext.
    /// </summary>
    public ProductUpdatePage()
    {
        ViewModel = App.GetService<ProductUpdateViewModel>();
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
    /// Handles the Update button click event to update the product details using the ViewModel.
    /// </summary>
    private async void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.UpdateProduct();
    }

    /// <summary>
    /// Handles the Mark Inactive button click event to mark the product as inactive using the ViewModel.
    /// </summary>
    private async void MarkInactiveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkProductInactive();
    }

    /// <summary>
    /// Handles the Mark Active button click event to mark the product as active using the ViewModel.
    /// </summary>
    private async void MarkActiveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkProductActive();
    }

    /// <summary>
    /// Handles the Update A Photo button click event to update the product photo using the ViewModel.
    /// </summary>
    public async void UpdateAPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.UpdateAPhoto();
    }
}
