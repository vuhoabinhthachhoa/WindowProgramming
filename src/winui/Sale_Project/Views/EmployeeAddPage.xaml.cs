using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// A page that allows adding a new employee.
/// </summary>
public sealed partial class EmployeeAddPage : Page
{
    /// <summary>
    /// Gets the ViewModel for the EmployeeAddPage.
    /// </summary>
    public EmployeeAddViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeAddPage"/> class.
    /// </summary>
    public EmployeeAddPage()
    {
        ViewModel = App.GetService<EmployeeAddViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    /// <summary>
    /// Handles the GoBack button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoBack();
    }

    /// <summary>
    /// Handles the Add button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.AddEmployee();
    }

    /// <summary>
    /// Handles the Register button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.Register();
    }
}
