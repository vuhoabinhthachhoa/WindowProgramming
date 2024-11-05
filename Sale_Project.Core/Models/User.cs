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
