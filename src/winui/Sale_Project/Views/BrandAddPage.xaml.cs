using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the BrandAddPage, providing UI logic for adding a new brand.
/// </summary>
public sealed partial class BrandAddPage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing data.
    /// </summary>
    public BrandAddViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the BrandAddPage, setting up the ViewModel and DataContext.
    /// </summary>
    public BrandAddPage()
    {
        ViewModel = App.GetService<BrandAddViewModel>();
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
    /// Handles the Add button click event to add a new brand using the ViewModel.
    /// </summary>
    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.AddBrand();
    }
}
