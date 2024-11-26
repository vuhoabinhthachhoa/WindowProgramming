using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employee;
using Sale_Project.Core.Models;
using Sale_Project.Services;
using System.Text.Json;
using RichardSzalay.MockHttp;
using System.Net.Http.Json;
using System.Net;
using System.Reflection.Metadata;

namespace Sale_Project.Tests.MSTest.ServiceTests;

[TestClass]
public class EmployeeServiceTests
{
    private Mock<IHttpService> _httpServiceMock;
    private Mock<IAuthService> _authServiceMock;
    private Mock<IDialogService> _dialogServiceMock;
    private EmployeeService _employeeService;
    private MockHttpMessageHandler _mockHttp;
    private const string token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsImlhdCI6MTczMTgyMzM4NywiZXhwIjoxNzMzNTUxMzg3LCJzY29wZSI6IkFETUlOIn0.RjX8tq9mMPzy4MTmMF7c9tLpkpinsVANV5phEJQL5OY";
    private HttpClient _httpClient;
    private IHttpService _httpService;

    [TestInitialize]
    public void SetUp()
    {

        _authServiceMock = new Mock<IAuthService>();
        _dialogServiceMock = new Mock<IDialogService>();
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/employee") };
        _httpService = new HttpService(_dialogServiceMock.Object);
        _employeeService = new EmployeeService(_httpClient, _httpService, _authServiceMock.Object, _dialogServiceMock.Object);
    }

    [TestMethod]
    public async Task AddEmployee_ValidRequest_ReturnsEmployee()
    {
        // Arrange
        var employeeCreationRequest = new EmployeeCreationRequest
        {
            Name = "Hoàng Thành",
            CitizenId = "123214324367",
            JobTitle = "Captain",
            Salary = 1000000
        };
       
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);
        // Act
        var result = await _employeeService.AddEmployee(employeeCreationRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hoàng Thành", result.Name);
        Assert.AreEqual("123214324367", result.CitizenId);
        Assert.AreEqual("Captain", result.JobTitle);
    }
    [TestMethod]
    public async Task UnemployedEmployee_ValidId_ReturnsTrue()
    {
        // Arrange
        long employeeId = 15;
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _employeeService.UnemployedEmployee(employeeId);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task UpdateEmployee_ValidEmployee_ReturnsUpdatedEmployee()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 15,
            Name = "Updated Name",
            CitizenId = "123456789",
            JobTitle = "Updated Job",
            Salary = 2000000
        };
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _employeeService.UpdateEmployee(employee);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Updated Name", result.Name);
        Assert.AreEqual("123456789", result.CitizenId);
        Assert.AreEqual("Updated Job", result.JobTitle);
    }

    [TestMethod]
    public async Task SearchEmployees_ValidRequest_ReturnsPagedData()
    {
        // Arrange
        int page = 1;
        int size = 10;
        string sortField = "name";
        SortType sortType = SortType.ASC;
        var searchRequest = new EmployeeSearchRequest { Name = "Test" };
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _employeeService.SearchEmployees(page, size, sortField, sortType, searchRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(PageData<Employee>));
    }

    [TestMethod]
    public async Task GetEmployeeById_ValidId_ReturnsEmployee()
    {
        // Arrange
        long employeeId = 1;
        _authServiceMock.Setup(x => x.GetAccessToken()).Returns(token);

        // Act
        var result = await _employeeService.GetEmployeeById(employeeId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(employeeId, result.Id);
    }


    
}
