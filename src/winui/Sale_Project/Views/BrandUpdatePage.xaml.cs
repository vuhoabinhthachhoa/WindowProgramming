using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.ViewModels;
using Windows.Storage.Pickers;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the BrandUpdatePage, providing UI logic for updating and managing brand details.
/// </summary>
public sealed partial class BrandUpdatePage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing brand update operations.
    /// </summary>
    public BrandUpdateViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the BrandUpdatePage, setting up the ViewModel and DataContext.
    /// </summary>
    public BrandUpdatePage()
    {
        ViewModel = App.GetService<BrandUpdateViewModel>();
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
    /// Handles the Update button click event to update the brand details using the ViewModel.
    /// </summary>
    private async void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.UpdateBrand();
    }

    /// <summary>
    /// Handles the Mark Inactive button click event to mark the brand as inactive using the ViewModel.
    /// </summary>
    private async void MarkInactiveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkBrandInactive();
    }
}
