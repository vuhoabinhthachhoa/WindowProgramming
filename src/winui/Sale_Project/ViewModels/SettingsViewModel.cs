﻿using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Views;
using Windows.ApplicationModel;

namespace Sale_Project.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    public ICommand SwitchThemeCommand
    {
        get;
    }
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IAuthService authService, IDialogService dialogService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();
        _authService = authService;
        _dialogService = dialogService;

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public async Task LogoutAsync()
    {
        _authService.Logout();
        await _dialogService.ShowSuccessAsync(
                   "Success",
                   "Logout successful!");
        App.MainWindow.Content = App.GetService<LoginPage>();
    }
}
