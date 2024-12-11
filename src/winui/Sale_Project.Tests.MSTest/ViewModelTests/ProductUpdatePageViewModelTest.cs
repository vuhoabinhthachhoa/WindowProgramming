using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Product;
using Sale_Project.Helpers;
using Sale_Project.Services;
using System.Net.Http.Json;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;
using System.Text;
using System.Text.Json;
using RichardSzalay.MockHttp;
using System.Reflection.Metadata;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Sale_Project.Tests.MSTest.ViewModelTests;
[TestClass]
public class ProductUpdatePageViewModelTest
{
    private Mock<IProductService> _mockProductService;
    private Mock<INavigationService> _mockNavigationService;
    private Mock<IDialogService> _mockDialogService;
    private Mock<ProductValidator> _mockProductValidator;
    private ProductUpdateViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>();
        _mockNavigationService = new Mock<INavigationService>();
        _mockDialogService = new Mock<IDialogService>();
        _mockProductValidator = new Mock<ProductValidator>(_mockDialogService.Object);
        _viewModel = new ProductUpdateViewModel(_mockProductService.Object, _mockNavigationService.Object, _mockDialogService.Object, _mockProductValidator.Object);
    }

    [TestMethod]
    public async Task OnNavigatedTo_ShouldSetCurrentProduct()
    {
        // Arrange
        var product = new Product { Code = "123", Name = "Test Product" };
        var productSearchRequest = new ProductSearchRequest { Code = product.Code, Name = product.Name };
        var expectedProduct = new Product { Code = "123", Name = "Test Product" };

        _mockProductService.Setup(s => s.GetSelectedProduct(It.IsAny<ProductSearchRequest>())).ReturnsAsync(expectedProduct);

        // Act
        _viewModel.OnNavigatedTo(product);

        // Assert
        Assert.AreEqual(expectedProduct, _viewModel.CurrentProduct);
    }

    [TestMethod]
    public void GoBack_ShouldNavigateToProductViewModel()
    {
        // Act
        _viewModel.GoBack();

        // Assert
        _mockNavigationService.Verify(s => s.NavigateTo(It.Is<string>(pageKey => pageKey == typeof(ProductViewModel).FullName), It.IsAny<object>(), false), Times.Once);
    }
}
