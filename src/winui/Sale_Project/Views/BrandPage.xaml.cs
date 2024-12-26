using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the BrandPage, providing UI logic for managing and displaying brands.
/// </summary>
public sealed partial class BrandPage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing brand data.
    /// </summary>
    public BrandViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the BrandPage, setting up the ViewModel and DataContext.
    /// </summary>
    public BrandPage()
    {
        ViewModel = App.GetService<BrandViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    /// <summary>
    /// Handles the Add Brand button click event to add a new brand using the ViewModel.
    /// </summary>
    private void AddBrandButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.AddBrand();
    }

    /// <summary>
    /// Handles the Search Brand button click event to search for brands using the ViewModel.
    /// </summary>
    private async void SearchBrandButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchBrand();
    }

    /// <summary>
    /// Handles the Previous button click event to navigate to the previous page of brands.
    /// </summary>
    private async void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToPreviousPage();
    }

    /// <summary>
    /// Handles the Next button click event to navigate to the next page of brands.
    /// </summary>
    private async void NextButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToNextPage();
    }

    /// <summary>
    /// Handles the Sort By ID Ascending button click event to sort brands by ID in ascending order.
    /// </summary>
    private async void SortByIDAscButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortByIDAsc();
    }

    /// <summary>
    /// Handles the Sort By ID Descending button click event to sort brands by ID in descending order.
    /// </summary>
    private async void SortByIDDescButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortByIDDesc();
    }

    private void ChangePage_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ChangePage.SelectedIndex == 1)
        {
            ViewModel.GoToProductPage();
        }
        else if (ChangePage.SelectedIndex == 2)
        {
            ViewModel.GoToCategoryPage();
        }
    }
}
