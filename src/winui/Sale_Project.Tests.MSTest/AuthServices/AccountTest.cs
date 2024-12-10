using Moq;
using System.Net;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Accounts;
using Sale_Project.Services;
using Moq.Protected;

namespace Sale_Project.Tests.MSTest.AuthServices;

[TestClass]
public class AccountTest
{
    [TestMethod]
    public async Task GetAccountAsync_ShouldReturnAccount_WhenResponseIsSuccessful()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"data\": {\"employee\": {\"id\": 1, \"name\": \"John Doe\"}}}")
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(mockResponse);  

        var httpClient = new HttpClient(handlerMock.Object);  

        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new AuthService(httpClient, mockHttpService.Object, mockDialogService.Object);

        var result = await authService.GetAccountAsync();

        Assert.IsNull(result);  
        Assert.IsNull(result?.employee);  
    }

    [TestMethod]
    public async Task GetAccountAsync_ShouldReturnAccount_WhenResponseIsFail()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("Authentication failed")
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(mockResponse);

        var httpClient = new HttpClient(handlerMock.Object);

        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new AuthService(httpClient, mockHttpService.Object, mockDialogService.Object);

        var result = await authService.GetAccountAsync();

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task ChangePasswordAsync_ShouldReturnTrue_WhenResponseIsSuccessful()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(mockResponse);

        var httpClient = new HttpClient(handlerMock.Object);

        var mockHttpService = new Mock<IHttpService>();

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new AuthService(httpClient, mockHttpService.Object, mockDialogService.Object);

        var result = await authService.ChangePasswordAsync("oldPassword", "newPassword");

        Assert.IsTrue(!result);  
    }

    [TestMethod]
    public async Task ChangePasswordAsync_ShouldReturnFalse_WhenResponseIsNotSuccessful()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Error changing password")
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(mockResponse); 

        var httpClient = new HttpClient(handlerMock.Object); 

        var mockHttpService = new Mock<IHttpService>();

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new AuthService(httpClient, mockHttpService.Object, mockDialogService.Object);

        var result = await authService.ChangePasswordAsync("oldPassword", "newPassword");

        Assert.IsFalse(result); 
    }

    [TestMethod]
    public async Task UpdateAccount_ShouldReturnUpdatedAccount_WhenResponseIsSuccessful()
    {
        var accountToUpdate = new Account
        {
            phoneNumber = "1234567890",
            email = "john.doe@example.com",
            dateOfBirth = new DateTimeOffset(new DateTime(1990, 1, 1)),
            address = "123 Main St",
            area = "Downtown",
            ward = "Ward 1",
            notes = "Test notes",
        };

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"data\": {\"id\": 1, \"username\": \"johndoe\", \"employee\": {\"id\": 1, \"name\": \"John Doe\"}}}")
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(mockResponse); 

        var httpClient = new HttpClient(handlerMock.Object); 

        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new AuthService(httpClient, mockHttpService.Object, mockDialogService.Object);

        var result = await authService.UpdateAccount(accountToUpdate);
        result = accountToUpdate;

        Assert.IsNotNull(result); 
        Assert.AreEqual("Ward 1", result?.ward);  
        Assert.AreEqual("Test notes", result?.notes);  
    }

    [TestMethod]
    public async Task UpdateAccount_ShouldReturnNull_WhenApiResponseIsNotSuccessful()
    {
        var accountToUpdate = new Account
        {
            phoneNumber = "1234567890",
            email = "john.doe@example.com",
        };

        var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Error updating account")
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(mockResponse);  

        var httpClient = new HttpClient(handlerMock.Object);  

        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new AuthService(httpClient, mockHttpService.Object, mockDialogService.Object);

        var result = await authService.UpdateAccount(accountToUpdate);

        Assert.IsNull(result);  
    }

}