using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.ViewModels;
using System;
using System.Threading.Tasks;
using OxyPlot;
using Sale_Project.Core.Models.Invoices;

namespace Sale_Project.Tests.MSTest.ViewModels;

[TestClass]
public class ReportViewModelTests
{
    private Mock<IInvoiceService> _mockInvoiceService;
    private ReportViewModel _viewModel;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockInvoiceService = new Mock<IInvoiceService>();
        _viewModel = new ReportViewModel(_mockInvoiceService.Object);
    }

    [TestMethod]
    public void Constructor_InitializesExpectedProperties()
    {
        // Check if the properties are initialized correctly
        Assert.IsNotNull(_viewModel.PlotModel);
        Assert.AreEqual("Top 10 Products by Revenue", _viewModel.PlotModel.Title);
        Assert.IsNotNull(_viewModel.DailyRevenuePlotModel);
        Assert.AreEqual("Daily Revenue", _viewModel.DailyRevenuePlotModel.Title);
        Assert.IsNotNull(_viewModel.TimeRange);
    }

    [TestMethod]
    public async Task LoadProductData_LoadsDataAndUpdatesPlotModel()
    {
        // Setup mock to return some dummy data
        _mockInvoiceService.Setup(service => service.GetAllInvoices(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(new List<Invoice> {
                new Invoice { /* Fill with test data */ }
            });

        _viewModel.LoadProductData();

        // Verify that the series and axes are added to the PlotModel
        Assert.IsTrue(_viewModel.PlotModel.Series.Count > 0);
        Assert.IsTrue(_viewModel.PlotModel.Axes.Count > 0);
    }

    [TestMethod]
    public async Task LoadDailyRevenueData_LoadsDataAndUpdatesDailyRevenuePlotModel()
    {
        // Setup mock to return some dummy data
        _mockInvoiceService.Setup(service => service.GetAllInvoices(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(new List<Invoice> {
                new Invoice { /* Fill with test data */ }
            });

        _viewModel.LoadDailyRevenueData();

        // Verify that the series and axes are added to the DailyRevenuePlotModel
        Assert.IsTrue(_viewModel.DailyRevenuePlotModel.Series.Count > 0);
        Assert.IsTrue(_viewModel.DailyRevenuePlotModel.Axes.Count > 0);
    }
}

