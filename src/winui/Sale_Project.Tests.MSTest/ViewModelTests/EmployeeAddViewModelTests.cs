using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employee;
using Sale_Project.Core.Models;
using Sale_Project.Helpers;
using Sale_Project.ViewModels;

namespace Sale_Project.Tests.MSTest.ViewModelTests;
[TestClass]
public class EmployeeAddViewModelTests
{
    private Mock<IEmployeeService> _employeeServiceMock;
    private Mock<INavigationService> _navigationServiceMock;
    private Mock<IDialogService> _dialogServiceMock;
    private Mock<IAuthService> _authServiceMock;
    private Mock<EmployeeCreationRequestValidator> _validatorMock;
    private EmployeeAddViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _employeeServiceMock = new Mock<IEmployeeService>();
        _navigationServiceMock = new Mock<INavigationService>();
        _dialogServiceMock = new Mock<IDialogService>();
        _authServiceMock = new Mock<IAuthService>();
        _validatorMock = new Mock<EmployeeCreationRequestValidator>(_dialogServiceMock.Object);

        _viewModel = new EmployeeAddViewModel(
            _employeeServiceMock.Object,
            _navigationServiceMock.Object,
            _dialogServiceMock.Object,
            _authServiceMock.Object,
            _validatorMock.Object
        );
    }

    [TestMethod]
    public void OnNavigatedTo_ShouldInitializeProperties()
    {
        // Act
        _viewModel.OnNavigatedTo(null);

        // Assert
        Assert.IsNotNull(_viewModel.EmployeeCreationRequest);
        Assert.IsNotNull(_viewModel.RegistrationRequest);
    }

    [TestMethod]
    public void OnNavigatedFrom_ShouldClearProperties()
    {
        // Arrange
        _viewModel.EmployeeCreationRequest = new EmployeeCreationRequest();
        _viewModel.RegistrationRequest = new RegistrationRequest();
        _viewModel.CreatedEmployee = new Employee();

        // Act
        _viewModel.OnNavigatedFrom();

        // Assert
        Assert.IsNull(_viewModel.EmployeeCreationRequest);
        Assert.IsNull(_viewModel.RegistrationRequest);
        Assert.IsNull(_viewModel.CreatedEmployee);
    }


    [TestMethod]
    public async Task AddEmployee_ShouldShowErrorIfAlreadyAdded()
    {
        // Arrange
        _viewModel.CreatedEmployee = new Employee { Id = 1 };

        // Act
        await _viewModel.AddEmployee();

        // Assert
        _dialogServiceMock.Verify(d => d.ShowErrorAsync("Error", "Employee has been already added!"), Times.Once);
        _employeeServiceMock.Verify(s => s.AddEmployee(It.IsAny<EmployeeCreationRequest>()), Times.Never);
    }


    

    [TestMethod]
    public async Task Register_ShouldShowErrorIfEmployeeNotAdded()
    {
        // Arrange
        _viewModel.CreatedEmployee = null;

        // Act
        await _viewModel.Register();

        // Assert
        _dialogServiceMock.Verify(d => d.ShowErrorAsync("Error", "Please add employee first!"), Times.Once);
    }

    [TestMethod]
    public async Task Register_ShouldShowErrorIfUsernameIsNull()
    {
        // Arrange
        _viewModel.CreatedEmployee = new Employee { Id = 1 };
        _viewModel.RegistrationRequest = new RegistrationRequest { Username = null };

        // Act
        await _viewModel.Register();

        // Assert
        _dialogServiceMock.Verify(d => d.ShowErrorAsync("Error", "Username cannot be null!"), Times.Once);
        _authServiceMock.Verify(a => a.Register(It.IsAny<RegistrationRequest>()), Times.Never);
    }

    [TestMethod]
    public async Task Register_ShouldRegisterAndShowSuccess()
    {
        // Arrange
        _viewModel.CreatedEmployee = new Employee { Id = 1 };
        _viewModel.RegistrationRequest = new RegistrationRequest { Username = "john_doe" };
        _authServiceMock.Setup(a => a.Register(It.IsAny<RegistrationRequest>())).ReturnsAsync(true);

        // Act
        await _viewModel.Register();

        // Assert
        _authServiceMock.Verify(a => a.Register(It.IsAny<RegistrationRequest>()), Times.Once);
        _dialogServiceMock.Verify(d => d.ShowSuccessAsync("Success", "Account for the employee registered successfully"), Times.Once);
    }

    [TestMethod]
    public async Task Register_ShouldDoNothingIfRegistrationFails()
    {
        // Arrange
        _viewModel.CreatedEmployee = new Employee { Id = 1 };
        _viewModel.RegistrationRequest = new RegistrationRequest { Username = "john_doe" };
        _authServiceMock.Setup(a => a.Register(It.IsAny<RegistrationRequest>())).ReturnsAsync(false);

        // Act
        await _viewModel.Register();

        // Assert
        _dialogServiceMock.Verify(d => d.ShowSuccessAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
