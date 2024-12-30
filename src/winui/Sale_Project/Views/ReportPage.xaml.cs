using Microsoft.UI.Xaml.Controls;
using OxyPlot;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

public sealed partial class ReportPage : Page
{
    public ReportViewModel ViewModel
    {
        get;
    }

    public ReportPage()
    {
        ViewModel = App.GetService<ReportViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void ChangeChart_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PlotView != null && DailyRevenuePlotModel != null)
        {

            if (ChangeChart_ComboBox.SelectedItem.ToString() == "Sales Report")
            {
                PlotView.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                DailyRevenuePlotModel.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }
            else
            {
                PlotView.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                DailyRevenuePlotModel.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }
    }

    private void ApplyTimeRangeButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.LoadProductData();
        ViewModel.LoadDailyRevenueData();
    }

    private void InvoiceToCsvButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.GenerateInvoicesCsv();
    }
}
