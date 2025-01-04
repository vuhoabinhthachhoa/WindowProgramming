using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the ProductPage, providing UI logic for managing and displaying products.
/// </summary>
public sealed partial class ProductPage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing product data.
    /// </summary>
    public ProductViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the ProductPage, setting up the ViewModel and DataContext.
    /// </summary>
    public ProductPage()
    {
        ViewModel = App.GetService<ProductViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    /// <summary>
    /// Handles the Add Product button click event to add a new product using the ViewModel.
    /// </summary>
    private void AddProductButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.AddProduct();
    }

    /// <summary>
    /// Handles the Search Product button click event to search for products using the ViewModel.
    /// </summary>
    private async void SearchProductButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchProduct();
    }

    /// <summary>
    /// Handles the Previous button click event to navigate to the previous page of products.
    /// </summary>
    private async void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToPreviousPage();
    }

    /// <summary>
    /// Handles the Next button click event to navigate to the next page of products.
    /// </summary>
    private async void NextButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToNextPage();
    }

    /// <summary>
    /// Handles the Sort By ID Ascending button click event to sort products by ID in ascending order.
    /// </summary>
    private async void SortByIDAscButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortByIDAsc();
    }

    /// <summary>
    /// Handles the Sort By ID Descending button click event to sort products by ID in descending order.
    /// </summary>
    private async void SortByIDDescButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortByIDDesc();
    }

    private void ChangePage_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ChangePage.SelectedIndex == 1)
        {
            ViewModel.GoToCategoryPage();
        }
        else if (ChangePage.SelectedIndex == 2)
        {
            ViewModel.GoToBrandPage();
        }
    }
}
