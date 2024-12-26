using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// Code-behind for the CategoryAddPage, providing UI logic for adding a new category.
/// </summary>
public sealed partial class CategoryAddPage : Page
{
    /// <summary>
    /// The ViewModel for this page, used for binding and managing data.
    /// </summary>
    public CategoryAddViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes the CategoryAddPage, setting up the ViewModel and DataContext.
    /// </summary>
    public CategoryAddPage()
    {
        ViewModel = App.GetService<CategoryAddViewModel>();
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
    /// Handles the Add button click event to add a new category using the ViewModel.
    /// </summary>
    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.AddCategory();
    }
}
