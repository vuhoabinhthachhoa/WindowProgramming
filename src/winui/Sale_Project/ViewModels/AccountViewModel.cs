using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Accounts;
using Windows.Storage;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Services;
using Sale_Project.Views;
using Microsoft.UI.Xaml;
using System.Text.Json;
using System.Reflection;
using Sale_Project.Helpers;
using System.Diagnostics;

namespace Sale_Project.ViewModels;

public partial class AccountViewModel : ObservableRecipient
{
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    public Dictionary<string, List<string>> Districts
    {
        get; set;
    }

    [ObservableProperty]
    private Account _account;

    public AccountViewModel(IAuthService authService, IDialogService dialogService)
    {
        _dialogService = dialogService;
        _authService = authService;
        LoadData();
        LoadAccountDataAsync();
    }

    /// <summary>
    /// Loads country and district data from the JSON files and deserializes them into appropriate collections.
    /// </summary>
    public void LoadData()
    {
        var path = FileHelper.GetJsonFilePath("Districts.json");
        var districtsJson = File.ReadAllText(path);
        Districts = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(districtsJson)!;
    }

    /// <summary>
    /// Retrieves the full file path for a given JSON file located in the "MockData" directory of the Sale_Project.
    /// </summary>
    /// <param name="fileName">The name of the JSON file to retrieve the path for.</param>
    /// <returns>
    /// The full file path to the specified JSON file.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the path to the "Sale_Project" directory cannot be found.
    /// </exception>
    //public string GetJsonFilePath(string fileName)
    //{
    //    var fullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    //    var index = fullPath.IndexOf(@"Sale_Project");

    //    if (index != -1)
    //    {
    //        var basePath = fullPath.Substring(0, index);

    //        return Path.Combine(basePath, @"Sale_Project\StaticData\", fileName);
    //    }
    //    else
    //    {
    //        throw new InvalidOperationException("Invalid path");
    //    }
    //}

    /// <summary>
    /// Loads account data asynchronously by retrieving the account information from the authentication service.
    /// </summary>
    /// <remarks>
    /// If the account data is successfully retrieved, the Account property is updated. 
    /// If the account data cannot be cast to the expected type, an exception will be thrown.
    /// </remarks>
    public async void LoadAccountDataAsync()
    {
        var account = await _authService.GetAccountAsync();
        if (account is Sale_Project.Core.Models.Accounts.Account validAccount)
        {
            Account = validAccount;
        }
        else
        {
            throw new InvalidCastException("The returned account is not of the expected type.");
        }
    }

    /// <summary>
    /// Changes the user's password by sending a request to the authentication service.
    /// </summary>
    /// <param name="oldPassword">The current password of the user.</param>
    /// <param name="newPassword">The new password to be set for the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// If either the old or new password is empty, an error dialog is displayed. 
    /// If the change is successful, a success message is shown, otherwise, an error message is displayed.
    /// </remarks>
    public async Task ChangePasswordAsync(string oldPassword, string newPassword)
    {
        try
        {
            //if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            //{
            //    await _dialogService.ShowErrorAsync(
            //        "Login Failed",
            //        "Username and password cannot be empty.");
            //    return;
            //}

            var result = await _authService.ChangePasswordAsync(oldPassword, newPassword);
            if (result)
            {
                await _dialogService.ShowSuccessAsync(
                    "Success",
                    "Password changed successfully!");
            }
            //else
            //{
            //    await _dialogService.ShowErrorAsync(
            //        "Change Password Failed",
            //        "Wrong password.");
            //}
        }
        catch (Exception ex)
        {
            var errorDialog = new ContentDialog
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

    /// <summary>
    /// Updates the user's account information by sending a request to the authentication service.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// If the update is successful, a success message is shown, otherwise, an error message is displayed.
    /// </returns>
    /// <remarks>
    /// If the update is successful, the Account property is updated and the account data is reloaded.
    /// </remarks>
    public async Task UpdateAccountAsync()
    {
        try
        {
            var result = await _authService.UpdateAccount(Account);
            if (result != null)
            {
                Account = result;
                LoadAccountDataAsync();
                await _dialogService.ShowSuccessAsync("Success", "Account updated successfully!");
            }
            else
            {
                await _dialogService.ShowErrorAsync("Update Failed", "Failed to update account.");
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
