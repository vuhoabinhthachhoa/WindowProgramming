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

namespace Sale_Project.Tests.MSTest.ViewModelTests
{
    [TestClass]
    public class ProductAddPageViewModelTest
    {
        private Mock<IProductService> _mockProductService;
        private Mock<IBranchService> _mockBranchService;
        private Mock<ICategoryService> _mockCategoryService;
        private Mock<INavigationService> _mockNavigationService;
        private Mock<IDialogService> _mockDialogService;
        private Mock<IAuthService> _mockAuthService;
        private Mock<ProductCreationRequestValidator> _mockProductCreationRequestValidator;
        private ProductAddViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockProductService = new Mock<IProductService>();
            _mockBranchService = new Mock<IBranchService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockNavigationService = new Mock<INavigationService>();
            _mockDialogService = new Mock<IDialogService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockProductCreationRequestValidator = new Mock<ProductCreationRequestValidator>(_mockDialogService.Object);

            _viewModel = new ProductAddViewModel(
                _mockProductService.Object,
                _mockBranchService.Object,
                _mockCategoryService.Object,
                _mockNavigationService.Object,
                _mockDialogService.Object,
                _mockAuthService.Object,
                _mockProductCreationRequestValidator.Object
            );
        }

        [TestMethod]
        public async Task OnNavigatedTo_ShouldInitializeProperties()
        {
            // Arrange
            var branches = new[] { new Branch { Name = "Branch1" }, new Branch { Name = "Branch2" } };
            var categories = new[] { new Category { Id = "Category1" }, new Category { Id = "Category2" } };

            _mockBranchService.Setup(service => service.GetAllBranches()).ReturnsAsync(branches);
            _mockCategoryService.Setup(service => service.GetAllCategories()).ReturnsAsync(categories);

            // Act
            await Task.Run(() => _viewModel.OnNavigatedTo(null));

            // Assert
            Assert.IsNotNull(_viewModel.ProductCreationRequest);
            CollectionAssert.AreEqual(branches.Select(b => b.Name).ToArray(), _viewModel.Branches);
            CollectionAssert.AreEqual(categories.Select(c => c.Id).ToArray(), _viewModel.Categories);
            Assert.IsNull(_viewModel.PickedImage);
        }

        [TestMethod]
        public void OnNavigatedFrom_ShouldResetProperties()
        {
            // Act
            _viewModel.OnNavigatedFrom();

            // Assert
            Assert.IsNull(_viewModel.ProductCreationRequest);
            Assert.IsNull(_viewModel.CreatedProduct);
            Assert.IsNull(_viewModel.PickedImage);
        }

        [TestMethod]
        public async Task AddProduct_ShouldShowErrorIfProductAlreadyAdded()
        {
            // Arrange
            _viewModel.CreatedProduct = new Product();

            // Act
            await _viewModel.AddProduct();

            // Assert
            _mockDialogService.Verify(service => service.ShowErrorAsync("Error", "Product has been already added!"), Times.Once);
        }
    }
}
