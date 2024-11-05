using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;

public class User : INotifyPropertyChanged
{
    private string username;
    private string password;
    private string email;
    private string userRole;
    private string storeName;

    public string Username
    {
        get => username;
        set
        {
            if (username != value)
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
    }

    public string Password
    {
        get => password;
        set
        {
            if (password != value)
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }

    public string Email
    {
        get => email;
        set
        {
            if (email != value)
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public string UserRole
    {
        get => userRole;
        set
        {
            if (userRole != value)
            {
                userRole = value;
                OnPropertyChanged(nameof(UserRole));
            }
        }
    }

    public string StoreName
    {
        get => storeName;
        set
        {
            if (storeName != value)
            {
                storeName = value;
                OnPropertyChanged(nameof(StoreName));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public static class UserManager
{
    private static User currentUser = new ();

    public static User CurrentUser
    {
        get => currentUser;
        set
        {
            if (currentUser != value)
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
    }

    public static event PropertyChangedEventHandler PropertyChanged;

    private static void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Đọc thông tin người dùng từ file user.json.
    /// </summary>
    /// <param name="filePath">Đường dẫn đến file user.json</param>
    /// <returns>Task hoàn thành việc đọc thông tin</returns>
    public static async Task LoadUserFromFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Không tìm thấy file user.json.", filePath);
        }

        try
        {
            var jsonContent = await File.ReadAllTextAsync(filePath);
            CurrentUser = JsonSerializer.Deserialize<User>(jsonContent) ?? new User();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi đọc file JSON: {ex.Message}");
        }
    }

    /// <summary>
    /// Lưu thông tin người dùng vào file user.json.
    /// </summary>
    /// <param name="filePath">Đường dẫn đến file user.json</param>
    /// <returns>Task hoàn thành việc lưu thông tin</returns>
    public static async Task SaveUserToFileAsync(string filePath)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(CurrentUser, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, jsonContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi ghi file JSON: {ex.Message}");
        }
    }
}
