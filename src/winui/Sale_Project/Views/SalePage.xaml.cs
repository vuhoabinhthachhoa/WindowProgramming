using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents; 
using Sale_Project.ViewModels;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Products;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Text;
using System.Diagnostics;
using Windows.System;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Helpers;
using System.Globalization;

namespace Sale_Project.Views;

public sealed partial class SalePage : Page
{
    public SaleViewModel ViewModel
    {
        get;
    }

    public SalePage()
    {
        ViewModel = App.GetService<SaleViewModel>();
        InitializeComponent();
    }

    private readonly List<(Product product, int quantity)> selectedProducts = new();
    private double totalAmount;
    private double totalDiscount;
    private double totalDue;
    private SampleCustomerDataType _selectedCustomer;

    /// <summary>
    /// Handles the click event of the "SearchProduct_TextChanged". 
    /// Searches for a product by its name and adds it to the selected products if not already selected.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="args">The event arguments.</param>
    private async void SearchProduct_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var searchText = sender.Text?.ToLower();

            await ViewModel.GetProducts();
            var products = ViewModel.Products;

            var filteredProducts = products.Where(p =>
                p.Name.ToLower().Contains(searchText)
            ).ToList();

            sender.ItemsSource = filteredProducts;
        }
    }

    /// <summary>
    /// Handles the click event of the "SearchProduct_SuggestionChosen". 
    /// Searches for a product by its name and adds it to the selected products if not already selected.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="args">The event arguments.</param>
    private void SearchProduct_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is Product selectedProduct)
        {
            ViewModel.Product = selectedProduct;

            var existingProduct = selectedProducts.FirstOrDefault(p => p.product.Id == ViewModel.Product.Id);

            if (existingProduct == default)
            {
                selectedProducts.Add((ViewModel.Product, 1));
                UpdateSelectedItemsDisplay();
            }
            sender.Text = string.Empty;
        }
    }

    /// <summary>
    /// Updates the display of selected products by adding product details and action buttons (increase, decrease, delete) to the UI.
    /// </summary>
    private void UpdateSelectedItemsDisplay()
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            SelectedItemsDisplayProduct.Children.Clear();
            foreach (var (product, quantity) in selectedProducts)
            {
                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 10),
                };

                var productImage = new Image
                {
                    Source = new BitmapImage(new Uri(product.ImageUrl)),
                    Width = 50,
                    Height = 50,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                var productInfo = new TextBlock
                {
                    TextAlignment = TextAlignment.Left,
                    Margin = new Thickness(20, 0, 0, 0),
                    Width = 300
                };

                productInfo.Inlines.Add(new Run
                {
                    Text = $"{product.Name}\n",
                    FontWeight = FontWeights.Bold,
                    FontSize = 18
                });

                productInfo.Inlines.Add(new Run
                {
                    Text = $"Discount: {product.DiscountPercent}%\nPrice: {product.SellingPrice}"
                });

                var quantityTextBox = new TextBox
                {
                    Text = quantity.ToString(),
                    Margin = new Thickness(5, 0, 0, 0),
                    Width = 5,
                    Height = 5,
                    TextAlignment = TextAlignment.Center,
                    IsReadOnly = true
                };

                var increaseButton = new Button
                {
                    Content = "+",
                    Tag = product.Id,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                increaseButton.Click += (sender, e) =>
                {
                    IncreaseQuantityButton_Click(sender, e);
                };

                var decreaseButton = new Button
                {
                    Content = "-",
                    Tag = product.Id,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                decreaseButton.Click += (sender, e) =>
                {
                    DecreaseQuantityButton_Click(sender, e);
                };

                var deleteButton = new Button
                {
                    Content = "X",
                    Tag = product.Id,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                deleteButton.Click += (sender, e) =>
                {
                    DeleteButton_Click(sender, e);
                };

                stackPanel.Children.Add(productImage);
                stackPanel.Children.Add(productInfo);
                stackPanel.Children.Add(decreaseButton);
                stackPanel.Children.Add(quantityTextBox);
                stackPanel.Children.Add(increaseButton);
                stackPanel.Children.Add(deleteButton);

                SelectedItemsDisplayProduct.Children.Add(stackPanel);
                CalculateTotals();
            }
        });
    }

    /// <summary>
    /// Handles the click event of the "IncreaseQuantityButton". 
    /// Increases the quantity of the selected product if the inventory allows it and updates the display.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private void IncreaseQuantityButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            var productItem = selectedProducts.FirstOrDefault(p => p.product.Id == productId);
            if (productItem.product != default)
            {
                if (productItem.product.InventoryQuantity > productItem.quantity)
                {
                    var index = selectedProducts.IndexOf(productItem);
                    selectedProducts[index] = (productItem.product, productItem.quantity + 1);
                    UpdateSelectedItemsDisplay();
                }
            }
        }
    }

    /// <summary>
    /// Handles the click event of the "DecreaseQuantityButton". 
    /// Decreases the quantity of the selected product, removes it if the quantity reaches zero, and updates the display.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private void DecreaseQuantityButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            var productItem = selectedProducts.FirstOrDefault(p => p.product.Id == productId);
            if (productItem.product != null)
            {
                if (productItem.quantity > 1)
                {
                    var index = selectedProducts.IndexOf(productItem);
                    selectedProducts[index] = (productItem.product, productItem.quantity - 1);
                }
                else
                {
                    selectedProducts.Remove(productItem);
                    CalculateTotals();
                }
                UpdateSelectedItemsDisplay();
            }
        }
    }

    /// <summary>
    /// Handles the click event of the "DeleteButton". 
    /// Removes the selected product from the list of selected products and updates the display.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            var productItem = selectedProducts.FirstOrDefault(p => p.product.Id == productId);
            if (productItem.product != null)
            {
                var index = selectedProducts.IndexOf(productItem);
                selectedProducts.Remove(productItem);
                CalculateTotals();
                UpdateSelectedItemsDisplay();
            }
        }
    }

    /// <summary>
    /// Calculates the total amount, total discount, and total due for the selected products and updates the display.
    /// </summary>
    private void CalculateTotals()
    {
        totalAmount = 0;
        totalDiscount = 0;
        totalDue = 0;

        foreach (var item in selectedProducts)
        {
            totalAmount += item.product.SellingPrice * item.quantity;
            totalDiscount += item.product.SellingPrice * item.product.DiscountPercent * item.quantity;
        }

        totalDue = totalAmount - totalDiscount;

        UpdateTotalsDisplay();
    }

    /// <summary>
    /// Updates the total amount, total discount, and total due displays with the calculated values.
    /// </summary>
    private void UpdateTotalsDisplay()
    {
        var totalAmountTextBlock = (TextBlock)FindName("TotalAmountTextBlock");
        var totalDiscountTextBlock = (TextBlock)FindName("TotalDiscountTextBlock");
        var totalDueTextBlock = (TextBlock)FindName("TotalDueTextBlock");
        var TotalAmount = (TextBlock)FindName("TotalAmount");
        var DueAmount = (TextBlock)FindName("DueAmount");

        var converter = new DoubleToCurrencyConverter();

        if (totalAmountTextBlock != null)
        {
            totalAmountTextBlock.Text = (string)converter.Convert(totalAmount, typeof(string), null, CultureInfo.CurrentCulture.Name);
        }

        if (totalDiscountTextBlock != null)
        {
            totalDiscountTextBlock.Text = (string)converter.Convert(totalDiscount, typeof(string), null, CultureInfo.CurrentCulture.Name);
        }

        if (totalDueTextBlock != null)
        {
            totalDueTextBlock.Text = (string)converter.Convert(totalDue, typeof(string), null, CultureInfo.CurrentCulture.Name);
        }

        if (TotalAmount != null)
        {
            TotalAmount.Text = (string)converter.Convert(totalDue, typeof(string), null, CultureInfo.CurrentCulture.Name);
        }

        if (DueAmount != null)
        {
            DueAmount.Text = (string)converter.Convert(totalDue, typeof(string), null, CultureInfo.CurrentCulture.Name);
        }
    }

    /// <summary>
    /// Handles the text change event of the AutoSuggestBox for customer search. 
    /// Filters the customer list based on the user's input and displays suitable results.
    /// </summary>
    /// <param name="sender">The AutoSuggestBox control that triggered the event.</param>
    /// <param name="args">The event arguments containing the reason for the text change.</param>
    private void AutoSuggestBox_TextChangedCustomer(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = ViewModel.customers
                .Where(e => e.FirstName.StartsWith(sender.Text, StringComparison.OrdinalIgnoreCase))
                .Select(e => $"{e.Id} \n{e.FirstName} {e.FamilyName} \n{e.Address} \n{e.State}, {e.City}\n{e.Email}\n{e.Zip}\n{e.Phone}\n")
                .ToList();

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }

            sender.ItemsSource = suitableItems;
        }
    }

    /// <summary>
    /// Handles the selection of a suggestion from the AutoSuggestBox.
    /// Sets the selected customer's details into the respective text fields.
    /// </summary>
    /// <param name="sender">The AutoSuggestBox control that triggered the event.</param>
    /// <param name="args">The event arguments containing the selected suggestion.</param>
    private void AutoSuggestBox_SuggestionChosenCustomer(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem.ToString() != "No results found")
        {
            var selectedCustomer = ViewModel.customers.FirstOrDefault(e =>
                $"{e.Id} \n{e.FirstName} {e.FamilyName} \n{e.Address} \n{e.State}, {e.City}\n{e.Email}\n{e.Zip}\n{e.Phone}\n" == args.SelectedItem.ToString());
            _selectedCustomer = selectedCustomer;
            OpenDetailsButton.IsEnabled = selectedCustomer != null;

            if (selectedCustomer != null)
            {
                NameTextBox.Text = $"{selectedCustomer.FirstName} {selectedCustomer.FamilyName}";
                PhoneTextBox.Text = selectedCustomer.Phone;
                AddressTextBox.Text = selectedCustomer.Address;
                CityTextBox.Text = selectedCustomer.City;
                DistrictTextBox.Text = selectedCustomer.State;
                VoucherTextBox.Text = selectedCustomer.Voucher;
            }

            sender.Text = string.Empty;
        }
    }

    /// <summary>
    /// Handles the click event of the "OpenDetailsButton" to navigate to the customer detail page.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private void OpenDetailsButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedCustomer != null)
        {
            Frame.Navigate(typeof(CustomerDetailPage), _selectedCustomer);
        }
    }

    /// <summary>
    /// Handles the click event of the "InstructionButton" to show the instruction view.
    /// Hides the content area and displays the instruction view.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private void InstructionButton_Click(object sender, RoutedEventArgs e)
    {
        InstructionFlipView.Visibility = Visibility.Visible;
        ContentArea.Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Handles the click event of the "EndInstructionButton" to hide the instruction view.
    /// Displays the content area and hides the instruction view.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private void EndInstructionButton_Click(object sender, RoutedEventArgs e)
    {
        InstructionFlipView.Visibility = Visibility.Collapsed;
        ContentArea.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Handles the click event of the "CreateInvoiceButton" to create an invoice.
    /// Extracts the selected product quantities and total amounts to create an invoice.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private async void CreateInvoiceButton_Click(object sender, RoutedEventArgs e)
    {
        var productQuantities = new Dictionary<string, int>();
        foreach (var item in selectedProducts)
        {
            if (item.product?.Id != null && item.quantity > 0)
            {
                productQuantities[item.product.Id.ToString()] = item.quantity;
            }
        }

        var converter = new DoubleToCurrencyConverter();

        var totalAmountText = TotalAmountTextBlock.Text;
        var totalDueText = TotalDueTextBlock.Text;

        var totalAmount = (double)converter.ConvertBack(totalAmountText, typeof(double), null, CultureInfo.CurrentCulture.Name);
        var totalDue = (double)converter.ConvertBack(totalDueText, typeof(double), null, CultureInfo.CurrentCulture.Name);

        await ViewModel.CreateInvoiceAsync(new InvoiceCreationRequest
        {
            totalAmount = (decimal)totalAmount,
            realAmount = (decimal)totalDue,
            productQuantity = productQuantities
        });
    }

    /// <summary>
    /// Handles the click event of the "FeedbackButton" to open the default mail client for user feedback.
    /// Prepares a mailto URI with the recipient, subject, and body for the email.
    /// </summary>
    /// <param name="sender">The sender of the click event.</param>
    /// <param name="e">The event arguments.</param>
    private async void FeedbackButton_Click(object sender, RoutedEventArgs e)
    {
        var recipient = "phongvan2032004@gmail.com";
        var subject = "Feedback from User";
        var body = "Please enter your feedback here...";

        var mailtoUri = new Uri($"mailto:{recipient}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}");

        await Launcher.LaunchUriAsync(mailtoUri);
    }
}
