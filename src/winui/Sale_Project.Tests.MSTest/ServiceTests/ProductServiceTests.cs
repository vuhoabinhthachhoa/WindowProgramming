using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Product;
using Sale_Project.Services;
using System.Net.Http.Json;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sale_Project.Core.Models;
using System.Text;
using System.Text.Json;
using RichardSzalay.MockHttp;
using System.Reflection.Metadata;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System;

namespace Sale_Project.Tests.MSTest.ServiceTests;

[TestClass]
public class ProductServiceTests
{
    private Mock<IHttpService> _httpServiceMock;
    private Mock<IAuthService> _authServiceMock;
    private Mock<IDialogService> _dialogServiceMock;
    private ProductService _productService;
    private const string token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsImlhdCI6MTczMTgyMzM4NywiZXhwIjoxNzMzNTUxMzg3LCJzY29wZSI6IkFETUlOIn0.RjX8tq9mMPzy4MTmMF7c9tLpkpinsVANV5phEJQL5OY";
    private MockHttpMessageHandler _mockHttp;
    private HttpClient _httpClient;
    private IHttpService _httpService;

    [TestInitialize]
    public void SetUp()
    {
        _authServiceMock = new Mock<IAuthService>();
        _dialogServiceMock = new Mock<IDialogService>();
        _httpService = new HttpService(_dialogServiceMock.Object);
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/product") };
        _productService = new ProductService(_httpClient, _httpService, _authServiceMock.Object, _dialogServiceMock.Object);
    }

    public static StreamContent GetMockImageFile()
    {
        // Create a simple in-memory image-like content
        byte[] mockImageBytes = GenerateMockImageBytes();

        // Convert byte array to MemoryStream
        var memoryStream = new MemoryStream(mockImageBytes);

        // Create StreamContent from MemoryStream
        var streamContent = new StreamContent(memoryStream);

        // Add headers to mimic an actual image file
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = "mockImage.png"
        };

        return streamContent;
    }

    private static byte[] GenerateMockImageBytes()
    {
        // Minimal valid PNG header and content (1x1 transparent pixel)
        return new byte[]
        {
            0x89, 0x50, 0x4E, 0x47, // PNG signature
            0x0D, 0x0A, 0x1A, 0x0A, // PNG signature continuation
            0x00, 0x00, 0x00, 0x0D, // IHDR chunk length
            0x49, 0x48, 0x44, 0x52, // IHDR chunk type
            0x00, 0x00, 0x00, 0x01, // Width: 1
            0x00, 0x00, 0x00, 0x01, // Height: 1
            0x08, 0x06, 0x00, 0x00, 0x00, // Bit depth, color type, compression, filter, interlace
            0x1F, 0x15, 0xC4, 0x89, // CRC for IHDR
            0x00, 0x00, 0x00, 0x0A, // IDAT chunk length
            0x49, 0x44, 0x41, 0x54, // IDAT chunk type
            0x78, 0x9C, 0x63, 0x60, // Compressed data
            0x00, 0x00, 0x00, 0x02, // Compressed data continuation
            0x00, 0x01, 0x5D, 0xC9, 0xB2, 0x22, // CRC for IDAT
            0x00, 0x00, 0x00, 0x00, // IEND chunk length
            0x49, 0x45, 0x4E, 0x44, // IEND chunk type
            0xAE, 0x42, 0x60, 0x82  // CRC for IEND
        };
    }


    [TestMethod]
    public async Task AddProduct_ValidRequest_ReturnsProduct()
    {
        //// Arrange
        //string mockFileName = "product-image.jpg";

        //// Debugging: Check the file name before passing it
        //Debug.Assert(mockFileName == "product-image.jpg", $"Unexpected file name: '{mockFileName}'");

        var productCreationRequest = new ProductCreationRequest
        {
            Data = new ProductData
            {
                Name = "Product A",
                CategoryId = "AO",
                ImportPrice = 50,
                SellingPrice = 100,
                BranchName = "channel",
                InventoryQuantity = 10,
                Size = "M",
                DiscountPercent = 0.5
            },
            File = GetMockImageFile()
        };

        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _productService.AddProduct(productCreationRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Product A", result.Name);
        Assert.AreEqual("AO", result.Category.ID);
        Assert.AreEqual(50, result.ImportPrice);
        Assert.AreEqual(100, result.SellingPrice);
        Assert.AreEqual("channel", result.Branch.Name);
        Assert.AreEqual(10, result.InventoryQuantity);
        Assert.AreEqual("M", result.Size);
        Assert.AreEqual(0.5, result.DiscountPercent);
    }

    [TestMethod]
    public async Task UpdateProduct_ValidProduct_ReturnsUpdatedProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            ImportPrice = 50,
            SellingPrice = 100,
            InventoryQuantity = 10,
            DiscountPercent = 0.5
        };
        var fileContent = GetMockImageFile();
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _productService.UpdateProduct(product, fileContent);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(50, result.ImportPrice);
        Assert.AreEqual(100, result.SellingPrice);
        Assert.AreEqual(10, result.InventoryQuantity);
        Assert.AreEqual(0.5, result.DiscountPercent);
    }

    [TestMethod]
    public async Task SearchProducts_ValidRequest_ReturnsPagedData()
    {
        // Arrange
        int page = 1;
        int size = 10;
        string sortField = "name";
        SortType sortType = SortType.ASC;
        var searchRequest = new ProductSearchRequest { Name = "Test" };
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _productService.SearchProducts(page, size, sortField, sortType, searchRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(PageData<Product>));
    }

    [TestMethod]
    public async Task GetSelectedProduct_ValidRequest_ReturnsProduct()
    {
        // Arrange
        var searchRequest = new ProductSearchRequest { Name = "Test Product 1" };
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _productService.GetSelectedProduct(searchRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test Product 1", result.Name);
    }

    [TestMethod]
    public async Task MarkInactiveProduct_ValidId_ReturnsTrue()
    {
        // Arrange
        long productId = 1;
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _productService.InactiveProduct(productId);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task MarkActiveProduct_ValidId_ReturnsTrue()
    {
        // Arrange
        long productId = 1;
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _productService.ActiveProduct(productId);

        // Assert
        Assert.IsTrue(result);
    }
}
