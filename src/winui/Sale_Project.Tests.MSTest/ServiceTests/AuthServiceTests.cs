using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sale_Project.Tests.MSTest.ServiceTests;


[TestClass]
public class AuthServiceTests
{
    private IHttpService _httpService;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    private AuthService _authService;
    private Mock<IDialogService> _dialogServiceMock;

    [TestInitialize]
    public void TestInitialize()
    {
        _dialogServiceMock = new Mock<IDialogService>();
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/auth") };
        _httpService = new HttpService(_dialogServiceMock.Object);

        _authService = new AuthService(_httpClient, _httpService);
    }

    [TestMethod]
    public async Task LoginAsync_InvalidCredentials_ReturnsFalse()
    {
        var username = "invalidUser";
        var password = "invalidPassword";

        bool result = await _authService.LoginAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task LoginAsync_EmptyUsername_ReturnsFalse()
    {
        var username = "";
        var password = "Lecaotuanvu@2004";

        bool result = await _authService.LoginAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task LoginAsync_EmptyPassword_ReturnsFalse()
    {
        var username = "user1";
        var password = "";

        bool result = await _authService.LoginAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task LoginAsync_NullUsername_ReturnsFalse()
    {
        string username = null;
        var password = "Lecaotuanvu@2004";

        bool result = await _authService.LoginAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task LoginAsync_NullPassword_ReturnsFalse()
    {
        var username = "user1";
        string password = null;

        bool result = await _authService.LoginAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }


    
}
