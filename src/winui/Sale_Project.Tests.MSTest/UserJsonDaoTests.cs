using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Sale_Project.Core.Models;
using Sale_Project.Contracts.Services;


namespace Sale_Project.Tests.MSTest;
[TestClass]
    public class UserJsonDaoTests
    {
        private UserJsonDao _userJsonDao;
        private string _testFilePath;

        [TestInitialize]
        public async Task TestInitialize()
        {
           _userJsonDao = new UserJsonDao();
           _testFilePath = _userJsonDao.GetJsonFilePath();
         }

        [TestCleanup]
        public void TestCleanup()
        {
            //// Cleanup the test file after each test
            //if (File.Exists(_testFilePath))
            //{
            //    File.Delete(_testFilePath);
            //}
        }

        private async Task CreateTestJsonFileAsync(List<User> users)
        {
            var jsonContent = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_testFilePath, jsonContent);
        }

        [TestMethod]
        public async Task GetUsersAsync_ShouldReturnUsers_WhenFileExistsAndIsValid()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Username = "user", Password = "user123", Email = "user@gmail.com" },
                new User { Username = "admin", Password = "admin123", Email = "admin@gmail.com" }
            };
            await CreateTestJsonFileAsync(users);

            

            // Act
            var result = await _userJsonDao.GetUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(users.Count, result.Count);
            for (int i = 0; i < users.Count; i++)
            {
                Assert.AreEqual(users[i].Username, result[i].Username);
                Assert.AreEqual(users[i].Password, result[i].Password);
            }
        }

    [TestMethod]
    public async Task AddUserAsync_ShouldAddNewUser_WhenInvoked()
    {
        // Arrange
        var initialUsers = new List<User>
    {
        new User { Username = "user", Password = "user123", Email = "user@gmail.com" }
    };
        await CreateTestJsonFileAsync(initialUsers);

        var newUser = new User { Username = "newUser", Password = "newUser123", Email = "newuser@gmail.com" };

        // Act
        await _userJsonDao.AddUserAsync(newUser);
        var result = await _userJsonDao.GetUsersAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(initialUsers.Count + 1, result.Count); // Ensure count increased by one
        Assert.AreEqual(newUser.Username, result[^1].Username);
        Assert.AreEqual(newUser.Password, result[^1].Password);
        Assert.AreEqual(newUser.Email, result[^1].Email);
    }

}
