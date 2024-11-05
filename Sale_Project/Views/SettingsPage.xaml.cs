using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;
using Sale_Project.Services;
namespace Sale_Project.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }


    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        var uiManager = App.GetService<UIManagerService>();

        if (uiManager.ShellPage != null)
        {
            uiManager.ShellPage.Startup.Visibility = Visibility.Visible;
            uiManager.ShellPage.Main.Visibility = Visibility.Collapsed;
        }
    }
}
