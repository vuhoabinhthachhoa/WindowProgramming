using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Sale_Project.Contracts.Services;

namespace Sale_Project.Services;
/// <summary>
/// Provides dialog services for displaying messages to the user.
/// </summary>
public class DialogService : IDialogService
{
    /// <summary>
    /// Shows a dialog with the specified title, message, and button text.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="message">The message content of the dialog.</param>
    /// <param name="buttonText">The text for the close button. Default is "OK".</param>
    private async Task ShowDialog(string title, string message, string buttonText = "OK")
    {
        try
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = buttonText,
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = App.MainWindow.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style
            };

            await dialog.ShowAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing dialog: {ex}");
        }
    }

    /// <summary>
    /// Shows an error dialog with the specified title and message.
    /// </summary>
    /// <param name="title">The title of the error dialog.</param>
    /// <param name="message">The message content of the error dialog.</param>
    public async Task ShowErrorAsync(string title, string message)
    {
        await ShowDialog(title, message);
    }

    /// <summary>
    /// Shows a warning dialog with the specified title and message.
    /// </summary>
    /// <param name="title">The title of the warning dialog.</param>
    /// <param name="message">The message content of the warning dialog.</param>
    public async Task ShowWarningAsync(string title, string message)
    {
        await ShowDialog(title, message);
    }

    /// <summary>
    /// Shows a success dialog with the specified title and message.
    /// </summary>
    /// <param name="title">The title of the success dialog.</param>
    /// <param name="message">The message content of the success dialog.</param>
    public async Task ShowSuccessAsync(string title, string message)
    {
        await ShowDialog(title, message);
    }

    /// <summary>
    /// Shows a confirmation dialog with the specified title and message.
    /// </summary>
    /// <param name="title">The title of the confirmation dialog.</param>
    /// <param name="message">The message content of the confirmation dialog.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user confirmed (true) or canceled (false).</returns>
    public async Task<bool> ShowConfirmAsync(string title, string message)
    {
        try
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = App.MainWindow.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing confirm dialog: {ex}");
            return false;
        }
    }
}
