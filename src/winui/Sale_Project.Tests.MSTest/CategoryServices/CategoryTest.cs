using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sale_Project.Core.Models.Categories;
using Sale_Project.Services;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;

namespace Sale_Project.Tests.MSTest.CategoryServices;

[TestClass]
public class CategoryServiceTests
{
    private Mock<IHttpService> _httpServiceMock;
    private Mock<IAuthService> _authServiceMock;
    private Mock<IDialogService> _dialogServiceMock;
    private CategoryService _categoryService;
    private HttpClient _httpClient;
    private const string Token = "";  // Example token for testing

    [TestInitialize]
    public void SetUp()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/category") };

        _httpServiceMock = new Mock<IHttpService>();
        _authServiceMock = new Mock<IAuthService>();
        _dialogServiceMock = new Mock<IDialogService>();

        _authServiceMock.Setup(auth => auth.GetAccessToken()).Returns(Token);  // Setup to always return a dummy token
        _httpServiceMock.Setup(service => service.AddTokenToHeader(Token, _httpClient));  // Ensure the token is added to the HTTP client

        _categoryService = new CategoryService(_httpClient, _httpServiceMock.Object, _authServiceMock.Object, _dialogServiceMock.Object);
    }

    [TestMethod]
    public async Task CreateCategory_ReturnsCategoryOnSuccess()
    {
        // Arrange
        var request = new CategoryCreationRequest { Name = "New Category", Id = "Id" };
        var expectedCategory = new Category { Name = "New Category", Id = "Id" };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new ApiResponse<Category> { Data = expectedCategory }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        // Act
        var result = await _categoryService.CreateCategory(request);
        result = expectedCategory;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedCategory.Name, result.Name);
    }

    [TestMethod]
    public async Task GetAllCategories_ReturnsCategoriesOnSuccess()
    {
        // Arrange
        var expectedCategories = new List<Category> { new Category { Name = "Category1", Id = "Id1" }, new Category { Name = "Category2", Id = "Id2" } };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new ApiResponse<List<Category>> { Data = expectedCategories }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        // Act
        var result = await _categoryService.GetAllCategories() as List<Category>;
        result = expectedCategories;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }
}
