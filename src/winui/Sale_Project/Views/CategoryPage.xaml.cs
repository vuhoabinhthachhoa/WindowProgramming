using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the CategoryPage, providing UI logic for managing and displaying categorys.
/// </summary>
public sealed partial class CategoryPage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing category data.
    /// </summary>
    public CategoryViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the CategoryPage, setting up the ViewModel and DataContext.
    /// </summary>
    public CategoryPage()
    {
        ViewModel = App.GetService<CategoryViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    /// <summary>
    /// Handles the Add Category button click event to add a new category using the ViewModel.
    /// </summary>
    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.AddCategory();
    }

    /// <summary>
    /// Handles the Previous button click event to navigate to the previous page of categorys.
    /// </summary>
    private async void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToPreviousPage();
    }

    /// <summary>
    /// Handles the Next button click event to navigate to the next page of categorys.
    /// </summary>
    private async void NextButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToNextPage();
    }

    /// <summary>
    /// Handles changes in the page selection via a selector component, navigating to different pages based on selection.
    /// </summary>
    private void ChangePage_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ChangePage.SelectedIndex == 1)
        {
            ViewModel.GoToProductPage(); // Navigates to the Product page.
        }
        else if (ChangePage.SelectedIndex == 2)
        {
            ViewModel.GoToBrandPage(); // Navigates to the Brand page.
        }
    }
}
