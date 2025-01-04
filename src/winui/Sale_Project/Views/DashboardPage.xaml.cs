using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Helpers;
using Sale_Project.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sale_Project.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();
        Alarm_GetProductsAlmostOutOfStock();
        Load();
    }

    /// <summary>
    /// Checks if any products are almost out of stock and displays notifications for them.
    /// </summary>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Retrieves the list of products by calling the <see cref="ViewModel.GetProducts"/> method.</item>
    /// <item>Filters the products with an inventory quantity less than 5 to identify those that are almost out of stock.</item>
    /// <item>Clears the existing notifications from the <see cref="NotificationStackPanel"/>.</item>
    /// <item>For each product almost out of stock, creates an <see cref="InfoBar"/> notification and adds it to the notification panel.</item>
    /// </list>
    /// </remarks>
    private async void Alarm_GetProductsAlmostOutOfStock()
    {
        await ViewModel.GetProducts();
        var products = ViewModel.Products;
        var productsAlmostOutOfStock = products.Where(p => p.InventoryQuantity > 5).ToList();
        NotificationStackPanel.Children.Clear();

        foreach (var product in productsAlmostOutOfStock)
        {
            var infoBar = new InfoBar
            {
                Title = "Product Almost Out of Stock",
                Message = $"Product {product.Name} is almost out of stock with only {product.InventoryQuantity} items left.",
                Severity = InfoBarSeverity.Warning,
                IsOpen = true,
                Margin = new Thickness(5),
                CornerRadius = new CornerRadius(30)
            };
            NotificationStackPanel.Children.Add(infoBar);
        }
    }

    /// <summary>
    /// Loads data for the current month, including invoice aggregation and employee total invoices, 
    /// and updates the revenue display.
    /// </summary>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Calculates the start and end dates for the current month.</item>
    /// <item>Retrieves invoice aggregation data for the current month by calling <see cref="ViewModel.GetInvoiceAggregationAsync"/>.</item>
    /// <item>Retrieves employee total invoice data for the current month by calling <see cref="ViewModel.GetEmployeeByTotalInvoiceAsync"/>.</item>
    /// <item>Converts the total amount from the invoice aggregation to a formatted currency string using <see cref="DoubleToCurrencyConverter"/>.</item>
    /// <item>Updates the revenue display with the formatted total amount.</item>
    /// </list>
    /// </remarks>
    private async void Load()
    {
        var now = DateTime.Now;
        var startDate = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
        var endDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).ToString("yyyy-MM-dd");

        await ViewModel.GetInvoiceAggregationAsync(startDate, endDate);
        await ViewModel.GetEmployeeByTotalInvoiceAsync(startDate, endDate);
        var converter = new DoubleToCurrencyConverter();
        Revenue.Text = (string)converter.Convert(ViewModel.Aggregation.totalAmount, typeof(string), null, CultureInfo.CurrentCulture.Name);
    }

    /// <summary>
    /// Handles the button click event for selecting a date range. It validates the selected dates 
    /// and performs necessary actions based on the selection.
    /// </summary>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Checks if the number of selected dates is less than two, showing an error if true.</item>
    /// <item>Checks if more than two dates are selected, showing a different error message if true.</item>
    /// <item>Retrieves the start and end dates from the selected dates if valid.</item>
    /// <item>Calls <see cref="ViewModel.GetEmployeeByTotalInvoiceAsync"/> to fetch employee data for the selected date range.</item>
    /// <item>Calls <see cref="ViewModel.GetInvoiceAggregationAsync"/> to fetch invoice aggregation data for the selected date range.</item>
    /// <item>Clears the selected dates after processing.</item>
    /// </list>
    /// </remarks>
    /// <param name="sender">
    /// The source of the event, typically the button that triggered this event.
    /// </param>
    /// <param name="e">
    /// Event data that provides information about the event.
    /// </param>
    private async void OnSelectButtonClick(object sender, RoutedEventArgs e)
    {
        var selectedDates = DateRangePicker.SelectedDates;

        if (selectedDates.Count < 2)
        {
            ErrorTextBlock.Text = "Please select both start and end date.";
            ErrorPopup.IsOpen = true;
            DateRangePicker.SelectedDates.Clear();
            return;
        }

        if (selectedDates.Count > 2)
        {
            ErrorTextBlock.Text = "You can only select two dates, a start date and an end date.";
            ErrorPopup.IsOpen = true;
            DateRangePicker.SelectedDates.Clear();
            return;
        }

        var startDate = selectedDates.First().Date.ToString("yyyy-MM-dd");
        var endDate = selectedDates.Last().Date.ToString("yyyy-MM-dd");

        await ViewModel.GetEmployeeByTotalInvoiceAsync(startDate, endDate);
        await ViewModel.GetInvoiceAggregationAsync(startDate, endDate);
        DateRangePicker.SelectedDates.Clear();
    }

    /// <summary>
    /// Opens the calendar popup when triggered by a user action.
    /// </summary>
    /// <remarks>
    /// This method sets the <see cref="Popup"/> control (represented by <see cref="CalendarPopup"/>)
    /// to be open, making the calendar visible to the user.
    /// </remarks>
    /// <param name="sender">
    /// The source of the event, typically the control (e.g., button) that triggered this event.
    /// </param>
    /// <param name="e">
    /// Event data that provides information about the event.
    /// </param>
    private void OpenCalendarPopup(object sender, RoutedEventArgs e)
    {
        CalendarPopup.IsOpen = true;
    }
}
