using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Accounts;
using Sale_Project.ViewModels;

namespace Sale_Project.Tests.MSTest.ViewModels;

[TestClass]
public class AccountViewModelTest
{
    [TestMethod]
    public void LoadData_ShouldLoadCountriesAndDistrictsCorrectly()
    {
        // Arrange
        var mockAuthService = new Mock<IAuthService>();
        var mockDialogService = new Mock<IDialogService>();
        var viewModel = new AccountViewModel(mockAuthService.Object, mockDialogService.Object);

        // Act
        viewModel.LoadData();

        // Assert
        Assert.IsTrue(viewModel.Countries.ContainsKey("Vietnam"));
        Assert.AreEqual("+84", viewModel.Countries["Vietnam"]);

        Assert.IsTrue(viewModel.Districts.ContainsKey("An Giang"));
        CollectionAssert.AreEquivalent(new List<string> { "Châu Đốc", "Châu Phú", "Tân Châu" }, viewModel.Districts["An Giang"]);
    }

    [TestMethod]
    public void GetJsonFilePath_ShouldReturnCorrectPath()
    {
        // Arrange
        var mockAuthService = new Mock<IAuthService>();
        var mockDialogService = new Mock<IDialogService>();
        var viewModel = new AccountViewModel(mockAuthService.Object, mockDialogService.Object);

        var fileName = "Test.json";
        var expectedPath = @"Sale_Project\MockData\Test.json";

        // Act
        var result = viewModel.GetJsonFilePath(fileName);

        // Assert
        StringAssert.Contains(result, expectedPath);
    }

    [TestMethod]
    public async Task LoadAccountDataAsync_ShouldSetAccount_WhenValidAccountReturned()
    {
        // Arrange
        var mockAuthService = new Mock<IAuthService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockAccount = new Account { username = "TestUser" };

        mockAuthService.Setup(auth => auth.GetAccountAsync()).ReturnsAsync(mockAccount);

        var viewModel = new AccountViewModel(mockAuthService.Object, mockDialogService.Object);

        // Act
        await Task.Run(() => viewModel.LoadAccountDataAsync());

        // Assert
        Assert.AreEqual(mockAccount, viewModel.Account);
    }

    [TestMethod]
    public async Task ChangePasswordAsync_ShouldShowError_WhenPasswordsAreEmpty()
    {
        // Arrange
        var mockAuthService = new Mock<IAuthService>();
        var mockDialogService = new Mock<IDialogService>();

        mockDialogService
            .Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var viewModel = new AccountViewModel(mockAuthService.Object, mockDialogService.Object);

        // Act
        await viewModel.ChangePasswordAsync("", "");

        // Assert
        mockDialogService.Verify(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAccountAsync_ShouldUpdateAccount_WhenSuccess()
    {
        // Arrange
        var mockAuthService = new Mock<IAuthService>();
        var mockDialogService = new Mock<IDialogService>();
        var updatedAccount = new Account { username = "UpdatedUser" };

        mockAuthService
            .Setup(auth => auth.UpdateAccount(It.IsAny<Account>()))
            .ReturnsAsync(updatedAccount);

        var viewModel = new AccountViewModel(mockAuthService.Object, mockDialogService.Object);

        // Act
        await viewModel.UpdateAccountAsync();

        // Assert
        Assert.AreEqual(updatedAccount, viewModel.Account);
        mockDialogService.Verify(dialog => dialog.ShowSuccessAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
