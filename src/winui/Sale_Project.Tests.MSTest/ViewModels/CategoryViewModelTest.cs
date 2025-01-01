using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Categories;
using Sale_Project.Services;
using Sale_Project.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sale_Project.Tests.MSTest.ViewModels;

[TestClass]
public class CategoryViewModelTests
{
    private Mock<ICategoryService> _mockCategoryService;
    private Mock<INavigationService> _mockNavigationService;
    private CategoryViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _mockCategoryService = new Mock<ICategoryService>();
        _mockNavigationService = new Mock<INavigationService>();
        _viewModel = new CategoryViewModel(_mockNavigationService.Object, _mockCategoryService.Object);
    }

    [TestMethod]
    public async Task OnNavigatedTo_LoadsDataSuccessfully()
    {
        // Arrange
        var categorys = new List<Category>
        {
            new Category { Id = "1", Name = "Category1" },
            new Category { Id = "2", Name = "Category2" }
        };
        _mockCategoryService.Setup(x => x.GetAllCategories()).ReturnsAsync(categorys);

        // Act
        _viewModel.OnNavigatedTo(null);

        // Assert
        Assert.AreEqual(2, _viewModel.Categories.Count);
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
    public void AddCategory_NavigatesToAddCategoryViewModel()
    {
        // Act
        _viewModel.AddCategory();

        // Assert
        _mockNavigationService.Verify(s => s.NavigateTo(It.Is<string>(pageKey => pageKey == typeof(CategoryAddViewModel).FullName), It.IsAny<object>(), false), Times.Once);
    }

    [TestMethod]
    public void OnSelectedCategoryChanged_NavigatesToUpdateCategoryViewModel()
    {
        // Arrange
        var category = new Category { Id = "1", Name = "Category1" };
        _viewModel.SelectedCategory = category;

        // Assert
        _mockNavigationService.Verify(s => s.NavigateTo(It.Is<string>(pageKey => pageKey == typeof(CategoryUpdateViewModel).FullName), It.IsAny<object>(), false), Times.Once);
    }
}
