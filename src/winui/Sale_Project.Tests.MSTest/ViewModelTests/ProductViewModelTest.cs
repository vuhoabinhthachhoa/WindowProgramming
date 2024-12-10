using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Product;
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

namespace Sale_Project.Tests.MSTest.ViewModelTests;
[TestClass]
public class ProductViewModelTest
{
    private Mock<IProductService> _mockProductService;
    private Mock<IBranchService> _mockBranchService;
    private Mock<ICategoryService> _mockCategoryService;
    private Mock<INavigationService> _mockNavigationService;
    private ProductViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>();
        _mockBranchService = new Mock<IBranchService>();
        _mockCategoryService = new Mock<ICategoryService>();
        _mockNavigationService = new Mock<INavigationService>();

        _viewModel = new ProductViewModel(
            _mockNavigationService.Object,
            _mockBranchService.Object,
            _mockCategoryService.Object,
            _mockProductService.Object);
    }

    [TestMethod]
    public async Task OnNavigatedTo_ShouldLoadBranchesAndCategories()
    {
        // Arrange
        var branches = new List<Branch> { new Branch { Name = "Branch1" }, new Branch { Name = "Branch2" } };
        var categories = new List<Category> { new Category { Name = "Category1" }, new Category { Name = "Category2" } };

        _mockBranchService.Setup(s => s.GetAllBranches()).ReturnsAsync(branches);
        _mockCategoryService.Setup(s => s.GetAllCategories()).ReturnsAsync(categories);

        // Act
        await Task.Run(() => _viewModel.OnNavigatedTo(null));

        // Assert
        Assert.AreEqual(2, _viewModel.Branches.Length);
        Assert.AreEqual(2, _viewModel.Categories.Length);
    }

    [TestMethod]
    public async Task LoadData_ShouldLoadProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Code = "P001",
                Name = "Product1",
                Category = new Category { Id = "C1", Name = "Category1" },
                ImportPrice = 10.0,
                SellingPrice = 15.0,
                Branch = new Branch { Id = 1, Name = "Branch1" },
                InventoryQuantity = 100,
                ImageUrl = "http://example.com/image1.jpg",
                CloudinaryImageId = "cloudinary1",
                BusinessStatus = true,
                Size = "M",
                DiscountPercent = 0.5
            },
            new Product
            {
                Id = 2,
                Code = "P002",
                Name = "Product2",
                Category = new Category { Id = "C2", Name = "Category2" },
                ImportPrice = 20.0,
                SellingPrice = 25.0,
                Branch = new Branch { Id = 2, Name = "Branch2" },
                InventoryQuantity = 200,
                ImageUrl = "http://example.com/image2.jpg",
                CloudinaryImageId = "cloudinary2",
                BusinessStatus = true,
                Size = "L",
                DiscountPercent = 0.5
            }
        };
        var pageData = new PageData<Product>
        {
            Data = products,
            TotalElements = 2,
            TotalPages = 1,
            Page = 1
        };

        _mockProductService.Setup(s => s.SearchProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<ProductSearchRequest>())).ReturnsAsync(pageData);

        // Act
        await _viewModel.LoadData();

        // Assert
        Assert.AreEqual(2, _viewModel.Products.Count);
        Assert.AreEqual(2, _viewModel.TotalItems);
        Assert.AreEqual(1, _viewModel.TotalPages);
        Assert.AreEqual(1, _viewModel.CurrentPage);
    }

    [TestMethod]
    public async Task GoToPage_ShouldChangeCurrentPageAndLoadData()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Code = "P001",
                Name = "Product1",
                Category = new Category { Id = "C1", Name = "Category1" },
                ImportPrice = 10.0,
                SellingPrice = 15.0,
                Branch = new Branch { Id = 1, Name = "Branch1" },
                InventoryQuantity = 100,
                ImageUrl = "http://example.com/image1.jpg",
                CloudinaryImageId = "cloudinary1",
                BusinessStatus = true,
                Size = "M",
                DiscountPercent = 0.5
            },
            new Product
            {
                Id = 2,
                Code = "P002",
                Name = "Product2",
                Category = new Category { Id = "C2", Name = "Category2" },
                ImportPrice = 20.0,
                SellingPrice = 25.0,
                Branch = new Branch { Id = 2, Name = "Branch2" },
                InventoryQuantity = 200,
                ImageUrl = "http://example.com/image2.jpg",
                CloudinaryImageId = "cloudinary2",
                BusinessStatus = true,
                Size = "L",
                DiscountPercent = 0.5
            }
        };
        var pageData = new PageData<Product>
        {
            Data = products,
            TotalElements = 2,
            TotalPages = 1,
            Page = 2
        };

        _mockProductService.Setup(s => s.SearchProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<ProductSearchRequest>())).ReturnsAsync(pageData);

        // Act
        await _viewModel.GoToPage(2);

        // Assert
        Assert.AreEqual(2, _viewModel.CurrentPage);
        Assert.AreEqual(2, _viewModel.Products.Count);
    }

    [TestMethod]
    public async Task SearchProduct_ShouldResetCurrentPageAndLoadData()
    {
        // Arrange
        var products = new List<Product>
            {
            new Product
            {
                Id = 1,
                Code = "P001",
                Name = "Product1",
                Category = new Category { Id = "C1", Name = "Category1" },
                ImportPrice = 10.0,
                SellingPrice = 15.0,
                Branch = new Branch { Id = 1, Name = "Branch1" },
                InventoryQuantity = 100,
                ImageUrl = "http://example.com/image1.jpg",
                CloudinaryImageId = "cloudinary1",
                BusinessStatus = true,
                Size = "M",
                DiscountPercent = 0.5
            },
            new Product
            {
                Id = 2,
                Code = "P002",
                Name = "Product2",
                Category = new Category { Id = "C2", Name = "Category2" },
                ImportPrice = 20.0,
                SellingPrice = 25.0,
                Branch = new Branch { Id = 2, Name = "Branch2" },
                InventoryQuantity = 200,
                ImageUrl = "http://example.com/image2.jpg",
                CloudinaryImageId = "cloudinary2",
                BusinessStatus = true,
                Size = "L",
                DiscountPercent = 0.5
            }
        };
        var pageData = new PageData<Product>
        {
            Data = products,
            TotalElements = 2,
            TotalPages = 1,
            Page = 1
        };

        _mockProductService.Setup(s => s.SearchProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<ProductSearchRequest>())).ReturnsAsync(pageData);

        // Act
        await _viewModel.SearchProduct();

        // Assert
        Assert.AreEqual(1, _viewModel.CurrentPage);
        Assert.AreEqual(2, _viewModel.Products.Count);
    }

    [TestMethod]
    public void AddProduct_ShouldNavigateToProductAddViewModel()
    {
        // Act
        _viewModel.AddProduct();

        // Assert
        _mockNavigationService.Verify(s => s.NavigateTo(It.Is<string>(pageKey => pageKey == typeof(ProductAddViewModel).FullName), It.IsAny<object>(), false), Times.Once);
    }

    [TestMethod]
    public async Task GoToPreviousPage_ShouldDecrementCurrentPageAndLoadData()
    {
        // Arrange
        _viewModel.CurrentPage = 2;
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Code = "P001",
                Name = "Product1",
                Category = new Category { Id = "C1", Name = "Category1" },
                ImportPrice = 10.0,
                SellingPrice = 15.0,
                Branch = new Branch { Id = 1, Name = "Branch1" },
                InventoryQuantity = 100,
                ImageUrl = "http://example.com/image1.jpg",
                CloudinaryImageId = "cloudinary1",
                BusinessStatus = true,
                Size = "M",
                DiscountPercent = 0.5
            },
            new Product
            {
                Id = 2,
                Code = "P002",
                Name = "Product2",
                Category = new Category { Id = "C2", Name = "Category2" },
                ImportPrice = 20.0,
                SellingPrice = 25.0,
                Branch = new Branch { Id = 2, Name = "Branch2" },
                InventoryQuantity = 200,
                ImageUrl = "http://example.com/image2.jpg",
                CloudinaryImageId = "cloudinary2",
                BusinessStatus = true,
                Size = "L",
                DiscountPercent = 0.5
            }
        };
        var pageData = new PageData<Product>
        {
            Data = products,
            TotalElements = 2,
            TotalPages = 1,
            Page = 1
        };

        _mockProductService.Setup(s => s.SearchProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<ProductSearchRequest>())).ReturnsAsync(pageData);

        // Act
        await _viewModel.LoadData();
        await _viewModel.GoToPreviousPage();

        // Assert
        Assert.AreEqual(1, _viewModel.CurrentPage);
        Assert.AreEqual(2, _viewModel.Products.Count);
    }

    [TestMethod]
    public async Task GoToNextPage_ShouldIncrementCurrentPageAndLoadData()
    {
        // Arrange
        _viewModel.CurrentPage = 1;
        var products = new List<Product>
            {
            new Product
            {
                Id = 1,
                Code = "P001",
                Name = "Product1",
                Category = new Category { Id = "C1", Name = "Category1" },
                ImportPrice = 10.0,
                SellingPrice = 15.0,
                Branch = new Branch { Id = 1, Name = "Branch1" },
                InventoryQuantity = 100,
                ImageUrl = "http://example.com/image1.jpg",
                CloudinaryImageId = "cloudinary1",
                BusinessStatus = true,
                Size = "M",
                DiscountPercent = 0.5
            },
            new Product
            {
                Id = 2,
                Code = "P002",
                Name = "Product2",
                Category = new Category { Id = "C2", Name = "Category2" },
                ImportPrice = 20.0,
                SellingPrice = 25.0,
                Branch = new Branch { Id = 2, Name = "Branch2" },
                InventoryQuantity = 200,
                ImageUrl = "http://example.com/image2.jpg",
                CloudinaryImageId = "cloudinary2",
                BusinessStatus = true,
                Size = "L",
                DiscountPercent = 0.5
            }
        };
        var pageData = new PageData<Product>
        {
            Data = products,
            TotalElements = 2,
            TotalPages = 2,
            Page = 2,
            Size = 1
        };

        _mockProductService.Setup(s => s.SearchProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<ProductSearchRequest>())).ReturnsAsync(pageData);

        // Act
        await _viewModel.LoadData();
        await _viewModel.GoToNextPage();

        // Assert
        Assert.AreEqual(2, _viewModel.CurrentPage);
        Assert.AreEqual(2, _viewModel.Products.Count);
    }
}