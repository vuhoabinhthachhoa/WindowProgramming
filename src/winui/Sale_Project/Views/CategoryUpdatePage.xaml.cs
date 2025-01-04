using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.ViewModels;
using Windows.Storage.Pickers;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the CategoryUpdatePage, providing UI logic for updating and managing category details.
/// </summary>
public sealed partial class CategoryUpdatePage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing category update operations.
    /// </summary>
    public CategoryUpdateViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the CategoryUpdatePage, setting up the ViewModel and DataContext.
    /// </summary>
    public CategoryUpdatePage()
    {
        ViewModel = App.GetService<CategoryUpdateViewModel>();
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
    /// Handles the Update button click event to update the category details using the ViewModel.
    /// </summary>
    private async void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.UpdateCategory();
    }

    /// <summary>
    /// Handles the Mark Inactive button click event to mark the category as inactive using the ViewModel.
    /// </summary>
    private async void MarkInactiveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkCategoryInactive();
    }
}
