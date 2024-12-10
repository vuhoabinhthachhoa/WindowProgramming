using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models.Accounts;
using Sale_Project.ViewModels;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Core;

namespace Sale_Project.Views;

public sealed partial class AccountPage : Page
{
    public AccountViewModel ViewModel
    {
        get;
    }

    public AccountPage()
    {
        ViewModel = App.GetService<AccountViewModel>();
        InitializeComponent();
        this.DataContext = ViewModel;
    }

    /// <summary>
    /// Handles the TextChanged event of the AutoSuggestBox for provinces.
    /// Filters the provinces based on user input and updates the suggestion list.
    /// </summary>
    /// <param name="sender">The AutoSuggestBox that triggered the event.</param>
    /// <param name="args">The event arguments containing the text change details.</param>
    private void AutoSuggestBox_TextChangedProvince(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");

            var allProvinces = ViewModel.Districts.Keys.ToList();

            foreach (var province in allProvinces)
            {
                var found = splitText.All((key) =>
                {
                    return province.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add(province);
                }
            }
            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }
    }

    /// <summary>
    /// Handles the TextChanged event of the AutoSuggestBox for districts.
    /// Filters the districts based on the selected province and user input.
    /// </summary>
    /// <param name="sender">The AutoSuggestBox that triggered the event.</param>
    /// <param name="args">The event arguments containing the text change details.</param>
    private void AutoSuggestBox_TextChangedDistrict(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        var selectedProvince = SuggestionOutputProvince.Text;
        List<string> filteredDistricts;

        if (!string.IsNullOrEmpty(selectedProvince) && ViewModel.Districts.ContainsKey(selectedProvince))
        {
            filteredDistricts = ViewModel.Districts[selectedProvince];
        }
        else
        {
            filteredDistricts = ViewModel.Districts.Values.SelectMany(d => d).ToList();
        }

        sender.ItemsSource = filteredDistricts.Where(d => d.Contains(sender.Text, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// Handles the SuggestionChosen event for selecting a province from the suggestions.
    /// Updates the TextBlock with the selected province.
    /// </summary>
    /// <param name="sender">The AutoSuggestBox that triggered the event.</param>
    /// <param name="args">The event arguments containing the selected suggestion.</param>
    private void AutoSuggestBox_SuggestionChosenProvince(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var suggestionOutput = FindName("SuggestionOutputProvince") as TextBlock;
        if (suggestionOutput != null)
        {
            suggestionOutput.Text = args.SelectedItem.ToString();
        }
        else
        {
            Debug.WriteLine("SuggestionOutput TextBlock not found.");
        }
    }

    /// <summary>
    /// Handles the SuggestionChosen event for selecting a district from the suggestions.
    /// Updates the TextBlock with the selected district.
    /// </summary>
    /// <param name="sender">The AutoSuggestBox that triggered the event.</param>
    /// <param name="args">The event arguments containing the selected suggestion.</param>
    private void AutoSuggestBox_SuggestionChosenDistrict(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var suggestionOutput = FindName("SuggestionOutputDistrict") as TextBlock;
        if (suggestionOutput != null)
        {
            suggestionOutput.Text = args.SelectedItem.ToString();
        }
        else
        {
            Debug.WriteLine("SuggestionOutput TextBlock not found.");
        }
    }

    /// <summary>
    /// Handles the SuggestionChosen event for selecting a country from the phone number suggestions.
    /// Updates the AutoSuggestBox with the selected country.
    /// </summary>
    /// <param name="sender">The AutoSuggestBox that triggered the event.</param>
    /// <param name="args">The event arguments containing the selected suggestion.</param>
    private void AutoSuggestBox_SuggestionChosenPhoneNumber(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var selectedCountry = args.SelectedItem.ToString();
        sender.Text = selectedCountry;
    }

    /// <summary>
    /// Handles the click event of the primary button in the ChangePasswordDialog.
    /// Validates the new password and changes the password if valid.
    /// </summary>
    /// <param name="sender">The ContentDialog that triggered the event.</param>
    /// <param name="args">The event arguments.</param>
    private async void ChangePasswordDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        var currentPassword = CurrentPasswordBox.Password;
        var newPassword = NewPasswordBox.Password;
        var confirmPassword = ConfirmPasswordBox.Password;

        if (newPassword == confirmPassword && ValidatePassword(newPassword))
        {
            await ViewModel.ChangePasswordAsync(currentPassword, newPassword);
        }
        else
        {
            ErrorMessageTextBlock.Text = "Inaccurate Password.";
            ErrorMessageTextBlock.Visibility = Visibility.Visible;

            await Task.Delay(5000);
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
        }
    }

    /// <summary>
    /// Validates the password based on the following criteria:
    /// - At least 8 characters long.
    /// - Contains at least one uppercase letter, one lowercase letter, one digit, and one special character.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if the password is valid, otherwise false.</returns>
    public bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        if (password.Length < 8) return false;

        if (!password.Any(ch => !char.IsLetterOrDigit(ch))) return false;

        if (!password.Any(ch => char.IsUpper(ch))) return false;

        if (!password.Any(ch => char.IsLower(ch))) return false;

        if (!password.Any(ch => char.IsDigit(ch))) return false;

        return true;
    }

    /// <summary>
    /// Handles the click event of the primary button in the VerificationDialog.
    /// Initiates the account update process.
    /// </summary>
    /// <param name="sender">The ContentDialog that triggered the event.</param>
    /// <param name="args">The event arguments.</param>
    private async void VerificationDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        await ViewModel.UpdateAccountAsync();
    }

    /// <summary>
    /// Handles the click event of the ChangePasswordButton. 
    /// Opens the ChangePasswordDialog to allow the user to change their password.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e) => await ChangePasswordDialog.ShowAsync();

    /// <summary>
    /// Handles the click event of the Verification button.
    /// Opens the VerificationDialog to allow the user to verify and update their account.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private async void Verification_Click(object sender, RoutedEventArgs e) => await VerificationDialog.ShowAsync();
}
