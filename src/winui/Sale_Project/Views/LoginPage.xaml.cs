using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Sale_Project.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel
    {
        get;
    }

    public LoginPage()
    {
        ViewModel = App.GetService<LoginViewModel>();
        this.InitializeComponent();
        LoadCredentials();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var username = UsernameBox.Text;
        var password = PasswordBox.Password;

        if (RememberMeCheckBox.IsChecked == true)
        {
            SaveCredentials(username, password);
        }
        else
        {
            RemoveCredentials();
        }

        await ViewModel.LoginAsync(username, password);
    }

    //private void RegisterButtonTextBlock_OnPointerPressed(Object sender, PointerRoutedEventArgs e)
    //{
    //    RegisterFeature.Visibility = Visibility.Visible;
    //    LoginFeature.Visibility = Visibility.Collapsed;
    //}

    //private void LoginButtonTextBlock_OnPointerPressed(Object sender, PointerRoutedEventArgs e)
    //{
    //    RegisterFeature.Visibility = Visibility.Collapsed;
    //    LoginFeature.Visibility = Visibility.Visible;
    //}


    private void LoadCredentials()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        if (localSettings.Values.ContainsKey("username"))
        {
            UsernameBox.Text = localSettings.Values["username"].ToString();
            RememberMeCheckBox.IsChecked = true; // Mark "Remember Me" if username exists
            LoadPassword(localSettings);
        }
    }

    private void LoadPassword(ApplicationDataContainer localSettings)
    {
        if (localSettings.Values.ContainsKey("password") && localSettings.Values.ContainsKey("entropy"))
        {
            string encryptedPasswordInBase64 = localSettings.Values["password"].ToString();
            string entropyInBase64 = localSettings.Values["entropy"].ToString();

            var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordInBase64);
            var entropyInBytes = Convert.FromBase64String(entropyInBase64);

            var passwordInBytes = ProtectedData.Unprotect(
                encryptedPasswordInBytes,
                entropyInBytes,
                DataProtectionScope.CurrentUser);

            PasswordBox.Password = Encoding.UTF8.GetString(passwordInBytes);
        }
    }

    private void SaveCredentials(string username, string password)
    {
        var passwordInBytes = Encoding.UTF8.GetBytes(password);
        var entropyInBytes = new byte[20];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(entropyInBytes);
        }

        var encryptedPassword = ProtectedData.Protect(
            passwordInBytes,
            entropyInBytes,
            DataProtectionScope.CurrentUser);

        var encryptedPasswordInBase64 = Convert.ToBase64String(encryptedPassword);
        var entropyInBase64 = Convert.ToBase64String(entropyInBytes);

        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["username"] = username;
        localSettings.Values["password"] = encryptedPasswordInBase64;
        localSettings.Values["entropy"] = entropyInBase64;
    }

    private void RemoveCredentials()
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values.Remove("username");
        localSettings.Values.Remove("password");
        localSettings.Values.Remove("entropy");
    }
}
