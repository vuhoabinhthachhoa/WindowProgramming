using Microsoft.UI.Xaml.Controls;
using OxyPlot;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// Represents the report page in the UI, handling report visualization and interactions.
/// </summary>
public sealed partial class ReportPage : Page
{
    /// <summary>
    /// Gets the view model for the report page.
    /// </summary>
    public ReportViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Constructor for the ReportPage. Initializes the view model and component.
    /// </summary>
    public ReportPage()
    {
        ViewModel = App.GetService<ReportViewModel>(); // Fetches the ReportViewModel from app services.
        InitializeComponent(); // Initializes UI components.
        DataContext = ViewModel; // Sets the data context for data binding.
    }

    /// <summary>
    /// Event handler for changing the chart selection. Updates visibility of charts based on selection.
    /// </summary>
    private void ChangeChart_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PlotView != null && DailyRevenuePlotModel != null)
        {
            if (ChangeChart_ComboBox.SelectedItem.ToString() == "Sales Report")
            {
                PlotView.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed; // Hides the PlotView.
                DailyRevenuePlotModel.Visibility = Microsoft.UI.Xaml.Visibility.Visible; // Shows the DailyRevenuePlotModel.
            }
            else
            {
                PlotView.Visibility = Microsoft.UI.Xaml.Visibility.Visible; // Shows the PlotView.
                DailyRevenuePlotModel.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed; // Hides the DailyRevenuePlotModel.
            }
        }
    }

    /// <summary>
    /// Event handler for applying time range filter to the report data.
    /// </summary>
    private void ApplyTimeRangeButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.LoadProductData(); // Loads product data.
        ViewModel.LoadDailyRevenueData(); // Loads daily revenue data.
    }

    /// <summary>
    /// Event handler for generating CSV files from invoices data.
    /// </summary>
    private void InvoiceToCsvButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.GenerateInvoicesCsv(); // Generates CSV from invoices.
    }
}
