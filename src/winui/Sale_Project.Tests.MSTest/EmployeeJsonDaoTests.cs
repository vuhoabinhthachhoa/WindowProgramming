using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sale_Project.Core.Models;
using Sale_Project.Services.Dao.JsonDao;
using static Sale_Project.Contracts.Services.IEmployeeDao;

namespace Sale_Project.Tests.MSTest
{
    [TestClass]
    public class EmployeeJsonDaoTests
    {
        private EmployeeJsonDao _employeeDao;
        private string _testFilePath;

        [TestInitialize]
        public async Task TestInitialize()
        {
            _employeeDao = new EmployeeJsonDao();
            _testFilePath = _employeeDao.GetJsonFilePath("employees.json");

            // Set up initial data for tests
            var initialEmployees = new List<Employee>
            {
                new Employee { ID = 1, Name = "Alice Smith", Phonenumber = "1234567890", Email = "alice@example.com", CitizenID = "A123456789", JobTitle = "Manager", Salary = 50000, DateOfBirth = DateOnly.Parse("1990-01-01"), Address = "123 Main St" },
                new Employee { ID = 2, Name = "Bob Johnson", Phonenumber = "0987654321", Email = "bob@example.com", CitizenID = "B987654321", JobTitle = "Engineer", Salary = 60000, DateOfBirth = DateOnly.Parse("1985-05-15"), Address = "456 Maple St" }
            };
            await CreateTestJsonFileAsync(initialEmployees);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Cleanup the test file after each test
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        private async Task CreateTestJsonFileAsync(List<Employee> employees)
        {
            var jsonContent = JsonSerializer.Serialize(employees);
            await File.WriteAllTextAsync(_testFilePath, jsonContent);
        }

        [TestMethod]
        public async Task GetEmployees_ShouldReturnEmployees_WhenFileExistsAndIsValid()
        {
            // Act
            var (employees, totalCount) = _employeeDao.GetEmployees(1, 10, "", new Dictionary<string, SortType>());

            // Assert
            Assert.IsNotNull(employees);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual(2, totalCount);
        }

        [TestMethod]
        public async Task AddEmployee_ShouldAddNewEmployee_WhenInvoked()
        {
            // Arrange
            var newEmployee = new Employee { ID = 3, Name = "Charlie Brown", Phonenumber = "5555555555", Email = "charlie@example.com", CitizenID = "C555555555", JobTitle = "Analyst", Salary = 45000, DateOfBirth = DateOnly.Parse("1992-03-01"), Address = "789 Oak St" };

            // Act
            var (isAdded, message) = _employeeDao.AddEmployee(newEmployee);
            var (employees, totalCount) = _employeeDao.GetEmployees(1, 10, "", new Dictionary<string, SortType>());

            // Assert
            Assert.IsTrue(isAdded);
            Assert.AreEqual(string.Empty, message);
            Assert.AreEqual(3, employees.Count);
            Assert.AreEqual("Charlie Brown", employees[^1].Name);
        }

        [TestMethod]
        public async Task DeleteEmployee_ShouldDeleteEmployee_WhenEmployeeExists()
        {
            // Arrange
            int employeeId = 1;

            // Act
            var result = _employeeDao.DeleteEmployee(employeeId);
            var (employees, totalCount) = _employeeDao.GetEmployees(1, 10, "", new Dictionary<string, SortType>());

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, employees.Count); // Verify one employee is removed
            Assert.IsTrue(employees.All(e => e.ID != employeeId));
        }

        [TestMethod]
        public async Task UpdateEmployee_ShouldUpdateEmployee_WhenEmployeeExists()
        {
            // Arrange
            var updatedEmployee = new Employee { ID = 1, Name = "Alice Johnson", Phonenumber = "1112223333", Email = "alice.johnson@example.com", CitizenID = "A111222333", JobTitle = "Director", Salary = 75000, DateOfBirth = DateOnly.Parse("1990-01-01"), Address = "123 New Address" };

            // Act
            var (isUpdated, message) = _employeeDao.UpdateEmployee(updatedEmployee);
            var (employees, totalCount) = _employeeDao.GetEmployees(1, 10, "", new Dictionary<string, SortType>());

            // Assert
            Assert.IsTrue(isUpdated);
            Assert.AreEqual(string.Empty, message);
            Assert.AreEqual("Alice Johnson", employees[0].Name);
            Assert.AreEqual("1112223333", employees[0].Phonenumber);
            Assert.AreEqual("Director", employees[0].JobTitle);
        }

        [TestMethod]
        public void IsValidInfo_ShouldReturnFalse_WhenEmployeeInfoIsInvalid()
        {
            // Arrange
            var invalidEmployee = new Employee { Name = "Invalid123", Phonenumber = "abc123", Email = "invalidemail", Salary = -100 };

            // Act
            var (isValid, message) = _employeeDao.IsValidInfo(invalidEmployee);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual("Invalid Name", message); // Only first error message is returned
        }
    }
}
