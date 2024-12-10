using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employee;
using Sale_Project.ViewModels;

namespace Sale_Project.Tests.MSTest.ViewModelTests;
[TestClass]
public class EmployeeViewModelTests
{
    private Mock<IEmployeeService> _employeeServiceMock;
    private Mock<INavigationService> _navigationServiceMock;
    private EmployeeViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _employeeServiceMock = new Mock<IEmployeeService>();
        _navigationServiceMock = new Mock<INavigationService>();
        _viewModel = new EmployeeViewModel(_navigationServiceMock.Object, _employeeServiceMock.Object);
    }

    [TestMethod]
    public async Task LoadData_ShouldPopulateEmployees_WhenDataIsReturned()
    {
        // Arrange
        var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe" },
                new Employee { Id = 2, Name = "Jane Smith" }
            };
        var employeePage = new PageData<Employee>
        {
            Data = employees,
            TotalElements = 2,
            TotalPages = 1,
            Page = 1
        };
        _employeeServiceMock
            .Setup(s => s.SearchEmployees(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<EmployeeSearchRequest>()))
            .ReturnsAsync(employeePage);

        // Act
        await _viewModel.LoadData();

        // Assert
        Assert.AreEqual(2, _viewModel.Employees.Count);
        Assert.AreEqual("John Doe", _viewModel.Employees[0].Name);
        Assert.AreEqual("Jane Smith", _viewModel.Employees[1].Name);
        Assert.AreEqual(2, _viewModel.TotalItems);
    }

    [TestMethod]
    public async Task SearchEmployee_ShouldResetCurrentPageAndLoadData()
    {
        // Arrange
        _viewModel.CurrentPage = 3;

        // Act
        await _viewModel.SearchEmployee();

        // Assert
        Assert.AreEqual(1, _viewModel.CurrentPage);
        _employeeServiceMock.Verify(s => s.SearchEmployees(1, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<EmployeeSearchRequest>()), Times.Once);
    }

    [TestMethod]
    public async Task GoToPage_ShouldChangeCurrentPageAndLoadData()
    {
        // Arrange
        int newPage = 2;

        // Act
        await _viewModel.GoToPage(newPage);

        // Assert
        Assert.AreEqual(newPage, _viewModel.CurrentPage);
        _employeeServiceMock.Verify(s => s.SearchEmployees(newPage, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<EmployeeSearchRequest>()), Times.Once);
    }

    [TestMethod]
    public async Task SortBySalaryAsc_ShouldUpdateSortFieldAndType()
    {
        // Arrange
        _viewModel.SortField = "name";
        _viewModel.SortType = SortType.DESC;

        // Act
        await _viewModel.SortBySalaryAsc();

        // Assert
        Assert.AreEqual("salary", _viewModel.SortField);
        Assert.AreEqual(SortType.ASC, _viewModel.SortType);
        _employeeServiceMock.Verify(s => s.SearchEmployees(It.IsAny<int>(), It.IsAny<int>(), "salary", SortType.ASC, It.IsAny<EmployeeSearchRequest>()), Times.Once);
    }


    [TestMethod]
    public async Task GoToNextPage_ShouldIncrementPageAndLoadData_WhenNotOnLastPage()
    {
        // Arrange
        _viewModel.CurrentPage = 1;
        _viewModel.TotalPages = 3;

        // Act
        await _viewModel.GoToNextPage();

        // Assert
        Assert.AreEqual(2, _viewModel.CurrentPage);
        _employeeServiceMock.Verify(s => s.SearchEmployees(2, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<EmployeeSearchRequest>()), Times.Once);
    }

    [TestMethod]
    public async Task GoToPreviousPage_ShouldDecrementPageAndLoadData_WhenNotOnFirstPage()
    {
        // Arrange
        _viewModel.CurrentPage = 2;

        // Act
        await _viewModel.GoToPreviousPage();

        // Assert
        Assert.AreEqual(1, _viewModel.CurrentPage);
        _employeeServiceMock.Verify(s => s.SearchEmployees(1, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SortType>(), It.IsAny<EmployeeSearchRequest>()), Times.Once);
    }
}
