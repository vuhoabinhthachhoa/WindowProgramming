using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// A page that allows updating employee details.
/// </summary>
public sealed partial class EmployeeUpdatePage : Page
{
    /// <summary>
    /// Gets the ViewModel for the EmployeeUpdatePage.
    /// </summary>
    public EmployeeUpdateViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeUpdatePage"/> class.
    /// </summary>
    public EmployeeUpdatePage()
    {
        ViewModel = App.GetService<EmployeeUpdateViewModel>();
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
    /// Handles the Update button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.UpdateEmployee();
    }

    /// <summary>
    /// Handles the MarkUnemployed button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void MarkUnemployedButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkEmployeeUnemployed();
    }
}
