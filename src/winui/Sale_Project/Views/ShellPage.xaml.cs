﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Sale_Project.Services;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.ViewModels;
using System.IO;
using System.Text.Json;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Sale_Project.Core.Models;

namespace Sale_Project.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    public Grid Startup => this._startupFeature;
    public Grid Main => this._mainFeature;

    private readonly Grid _startupFeature;
    private readonly Grid _mainFeature;

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        var uiManager = App.GetService<UIManagerService>();
        uiManager.ShellPage = this;

        _startupFeature = (Grid)FindName("StartupFeature");
        _mainFeature = (Grid)FindName("MainFeature");

        ViewModel.NavigationService.Frame = NavigationFrame;

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));

        ShellMenuBarSettingsButton.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerPressed), true);
        ShellMenuBarSettingsButton.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerReleased), true);
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerPressedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerPressed);
        ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerReleasedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerReleased);
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void ShellMenuBarSettingsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "PointerOver");
    }

    private void ShellMenuBarSettingsButton_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "Pressed");
    }

    private void ShellMenuBarSettingsButton_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "Normal");
    }

    private void ShellMenuBarSettingsButton_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "Normal");
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Password;

        var filePath = @"C:\Users\Admin\source\repos\Sale_Project\Sale_Project\Repository\UserManager.json";
        System.Diagnostics.Debug.WriteLine($"File path: {filePath}");
        if (!File.Exists(filePath))
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = "User data file not found.",
                CloseButtonText = "OK"
            };

            if (errorDialog.XamlRoot == null)
            {
                errorDialog.XamlRoot = this.XamlRoot;
            }

            await errorDialog.ShowAsync();
            return;
        }

        var jsonContent = await File.ReadAllTextAsync(filePath);
        var users = JsonSerializer.Deserialize<List<Sale_Project.Core.Models.User>>(jsonContent);

        var user = users?.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            UserManager.CurrentUser = user;
            Startup.Visibility = Visibility.Collapsed;
            Main.Visibility = Visibility.Visible;
            
            System.Diagnostics.Debug.WriteLine($"CurrentUser: {UserManager.CurrentUser.Username} {UserManager.CurrentUser.UserRole}");
        }
        else
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Login Failed",
                Content = "Invalid username or password.",
                CloseButtonText = "Ok"
            };

            if (dialog.XamlRoot == null)
            {
                dialog.XamlRoot = this.XamlRoot;
            }

            await dialog.ShowAsync();
        }
    }

    private async void RegisterButton_Click_Async(object sender, RoutedEventArgs e)
    {
        var username = NewUsernameTextBox.Text;  
        var password = NewPasswordBox.Password;
        var email = NewEmailTextBox.Text;
        var storeName = NewStoreNameTextBox.Text;

        var filePath = @"C:\Users\Admin\source\repos\Sale_Project\Sale_Project\Repository\UserManager.json";

        var users = new List<Sale_Project.Core.Models.User>();

        if (File.Exists(filePath))
        {
            var jsonContent = await File.ReadAllTextAsync(filePath);
            var existingUsers = JsonSerializer.Deserialize<List<Sale_Project.Core.Models.User>>(jsonContent);
            if (existingUsers != null)
            {
                users = existingUsers;
            }
        }

        var isStoreNameNew = !users.Exists(user => user.StoreName == storeName);

        var newUser = new Sale_Project.Core.Models.User
        {
            Username = username,
            Password = password,
            Email = email,
            StoreName = storeName,
            UserRole = isStoreNameNew ? "Admin" : "User" 
        };

        users.Add(newUser);

        var updatedJsonContent = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, updatedJsonContent);

        RegisterFeature.Visibility = Visibility.Collapsed;
        LoginFeature.Visibility = Visibility.Visible;
    }

    private void RegisterButtonTextBlock_OnPointerPressed(Object sender, PointerRoutedEventArgs e)
    {
        RegisterFeature.Visibility = Visibility.Visible;
        LoginFeature.Visibility = Visibility.Collapsed;
    }

    private void LoginButtonTextBlock_OnPointerPressed(Object sender, PointerRoutedEventArgs e)
    {
        RegisterFeature.Visibility = Visibility.Collapsed;
        LoginFeature.Visibility = Visibility.Visible;
    }
}
