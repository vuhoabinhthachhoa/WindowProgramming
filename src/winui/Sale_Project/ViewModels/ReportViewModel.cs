using CommunityToolkit.Mvvm.ComponentModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employees;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for handling the reports related to invoices and plotting revenue data.
/// </summary>
public partial class ReportViewModel : ObservableRecipient
{
    private readonly IInvoiceService _invoiceService;

    [ObservableProperty]
    private TimeRange _timeRange;

    [ObservableProperty]
    private PlotModel _plotModel;

    [ObservableProperty]
    private PlotModel _dailyRevenuePlotModel;

    /// <summary>
    /// Initializes a new instance of the ReportViewModel class.
    /// </summary>
    /// <param name="invoiceService">Service to interact with invoice data.</param>
    public ReportViewModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        TimeRange = new TimeRange();
        PlotModel = new PlotModel { Title = "Top 10 Products by Revenue" };
        DailyRevenuePlotModel = new PlotModel { Title = "Daily Revenue" };
        LoadProductData();
        LoadDailyRevenueData();
    }

    /// <summary>
    /// Loads and plots product data based on total revenue within a specified time range.
    /// </summary>
    public async void LoadProductData()
    {
        var invoices = await _invoiceService.GetAllInvoices(TimeRange.StartDate, TimeRange.EndDate);

        if (invoices != null)
        {
            var productRevenue = invoices
                .SelectMany(i => i.InvoiceDetails)
                .GroupBy(d => d.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalRevenue = g.Sum(d => d.SellingPrice * d.Quantity)
                })
                .OrderByDescending(p => p.TotalRevenue)
                .Take(10)
                .ToList();

            PlotModel.Series.Clear();
            var series = new BarSeries { Title = "Revenue" };

            foreach (var product in productRevenue)
            {
                series.Items.Add(new BarItem { Value = (double)product.TotalRevenue });
            }

            PlotModel.Series.Add(series);

            PlotModel.Axes.Clear();
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.AddRange(productRevenue.Select(p => p.ProductName));
            PlotModel.Axes.Add(categoryAxis);

            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });

            PlotModel.InvalidatePlot(true);
        }
    }

    /// <summary>
    /// Loads and plots daily revenue data based on real amount totals by day within a specified time range.
    /// </summary>
    public async void LoadDailyRevenueData()
    {
        var invoices = await _invoiceService.GetAllInvoices(TimeRange.StartDate, TimeRange.EndDate);

        if (invoices != null)
        {
            var dailyRevenue = invoices
                .GroupBy(i => i.CreatedDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(i => i.RealAmount)
                })
                .OrderBy(d => d.Date)
                .ToList();

            DailyRevenuePlotModel.Series.Clear();
            var series = new BarSeries { Title = "Daily Revenue", IsStacked = false };

            int categoryIndex = 0;
            foreach (var day in dailyRevenue)
            {
                series.Items.Add(new BarItem { Value = (double)day.TotalRevenue, CategoryIndex = categoryIndex++ });
            }

            DailyRevenuePlotModel.Series.Add(series);

            DailyRevenuePlotModel.Axes.Clear();
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left, };
            categoryAxis.Labels.AddRange(dailyRevenue.Select(d => d.Date.ToString("MM/dd")));
            DailyRevenuePlotModel.Axes.Add(categoryAxis);
            DailyRevenuePlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Revenue" });

            DailyRevenuePlotModel.InvalidatePlot(true);
        }
    }

    /// <summary>
    /// Generates CSV files of invoices within a specified time range.
    /// </summary>
    public async void GenerateInvoicesCsv()
    {
        await _invoiceService.GenerateInvoicesCsv(TimeRange.StartDate, TimeRange.EndDate);
    }
}
