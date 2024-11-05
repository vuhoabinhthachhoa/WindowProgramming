// JsonDao.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;

public class UserJsonDao : IDao
{
    private readonly string _filePath;

    public UserJsonDao()
    {
        _filePath = GetJsonFilePath();
    }

    public string GetJsonFilePath()
    {
        var fullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var index = fullPath?.IndexOf(@"Sale_Project");

        if (index.HasValue && index.Value != -1 && fullPath != null)
        {
            var basePath = fullPath.Substring(0, index.Value);
            return Path.Combine(basePath, @"Sale_Project\MockData\UserManager.json");
        }
        else
        {
            throw new InvalidOperationException("The path does not contain 'Sale_Project'.");
        }
    }

    public async Task<List<Sale_Project.Core.Models.User>> GetUsersAsync()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("User data file not found.", _filePath);
        }

        var jsonContent = await File.ReadAllTextAsync(_filePath);
        var users = JsonSerializer.Deserialize<List<Sale_Project.Core.Models.User>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (users == null)
        {
            throw new InvalidDataException("Deserialized user data is null.");
        }

        return users;
    }

    public async Task SaveUsersAsync(List<User> users)
    {
        var filePath = GetJsonFilePath();
        var updatedJsonContent = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, updatedJsonContent);
    }

    public async Task RegisterUserAsync(User newUser)
    {
        var users = await GetUsersAsync();
        users.Add(newUser);
        await SaveUsersAsync(users);
    }
}