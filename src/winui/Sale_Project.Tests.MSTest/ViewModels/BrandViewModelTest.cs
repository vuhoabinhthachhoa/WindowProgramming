using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Brands;
using Sale_Project.Services;
using Sale_Project.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sale_Project.Tests.MSTest.ViewModels;

[TestClass]
public class BrandViewModelTests
{
    private Mock<IBrandService> _mockBrandService;
    private Mock<INavigationService> _mockNavigationService;
    private BrandViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _mockBrandService = new Mock<IBrandService>();
        _mockNavigationService = new Mock<INavigationService>();
        _viewModel = new BrandViewModel(_mockNavigationService.Object, _mockBrandService.Object);
    }

    [TestMethod]
    public async Task OnNavigatedTo_LoadsDataSuccessfully()
    {
        // Arrange
        var brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Brand1" },
            new Brand { Id = 2, Name = "Brand2" }
        };
        _mockBrandService.Setup(x => x.GetAllBrands()).ReturnsAsync(brands);

        // Act
        _viewModel.OnNavigatedTo(null);

        // Assert
        Assert.AreEqual(2, _viewModel.Brands.Count);
        Assert.AreEqual(2, _viewModel.TotalItems);
        Assert.AreEqual(1, _viewModel.TotalPages); // Assuming rows per page is 5 and we only have 2 items.
    }

    [TestMethod]
    public async Task GoToNextPage_NavigatesCorrectly()
    {

        _viewModel.CurrentPage = 1;
        _viewModel.TotalPages = 2; // Assuming there are enough items for 2 pages

        // Act
        await _viewModel.GoToNextPage();

        // Assert
        Assert.AreEqual(1, _viewModel.CurrentPage);
    }

    [TestMethod]
    public async Task GoToPreviousPage_NavigatesCorrectly()
    {
        // Set initial conditions
        _viewModel.CurrentPage = 2; // Assuming the current page is 2

        // Act
        await _viewModel.GoToPreviousPage();

        // Assert
        Assert.AreEqual(1, _viewModel.CurrentPage);
    }

    [TestMethod]
    public void AddBrand_NavigatesToAddBrandViewModel()
    {
        // Act
        _viewModel.AddBrand();

        // Assert
        _mockNavigationService.Verify(s => s.NavigateTo(It.Is<string>(pageKey => pageKey == typeof(BrandAddViewModel).FullName), It.IsAny<object>(), false), Times.Once);
    }

    [TestMethod]
    public void OnSelectedBrandChanged_NavigatesToUpdateBrandViewModel()
    {
        // Arrange
        var brand = new Brand { Id = 1, Name = "Brand1" };
        _viewModel.SelectedBrand = brand;

        // Assert
        _mockNavigationService.Verify(s => s.NavigateTo(It.Is<string>(pageKey => pageKey == typeof(BrandUpdateViewModel).FullName), It.IsAny<object>(), false), Times.Once);
    }
}
