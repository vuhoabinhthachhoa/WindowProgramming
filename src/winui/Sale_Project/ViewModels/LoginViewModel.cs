using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Sale_Project.Contracts.Services;
using Sale_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Views;

namespace Sale_Project.ViewModels;
public partial class LoginViewModel : ObservableRecipient
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public LoginViewModel(IAuthService authService, INavigationService navigationService, IDialogService dialogService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }

    public async Task LoginAsync(string username, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await _dialogService.ShowErrorAsync(
                    "Login Failed",
                    "Username and password cannot be empty.");
                return;
            }

            var loginSuccessful = await _authService.LoginAsync(username, password);
            if (loginSuccessful)
            {

                //var uiManager = App.GetService<UIManagerService>();

                //if (uiManager.ShellPage != null)
                //{
                //    uimanager.shellpage.startup.visibility = visibility.collapsed;
                //    uimanager.shellpage.main.visibility = visibility.visible;

                //}
                //_navigationService.NavigateTo(typeof(DashboardViewModel).FullName!);
                var shell = App.GetService<ShellPage>();
                App.MainWindow.Content = shell;

                if (shell.FindName("NavigationFrame") is Frame shellFrame)
                {
                    _navigationService.Frame = shellFrame;
                    _navigationService.NavigateTo(typeof(DashboardViewModel).FullName!);
                }
                await _dialogService.ShowSuccessAsync(
                   "Success",
                   "Login successful!");
            }

            else
            {
                await _dialogService.ShowErrorAsync(
                   "Login Failed",
                   "Invalid username or password.");
            }
        }
        catch (Exception ex)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = $"An error occurred: {ex.Message}",
                CloseButtonText = "OK"
            };

            if (errorDialog.XamlRoot == null)
            {
                errorDialog.XamlRoot = App.MainWindow.Content.XamlRoot;
            }

            await errorDialog.ShowAsync();
        }


    }

}
