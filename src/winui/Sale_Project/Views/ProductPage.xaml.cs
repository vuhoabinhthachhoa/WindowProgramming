//using Microsoft.UI.Xaml.Controls;
//using Sale_Project.Core.Contracts.Services;
//using Sale_Project.Core.Models;
//using Sale_Project.ViewModels;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Sale_Project.Views
//{
//    public sealed partial class ProductPage : Page
//    {
//        public ProductViewModel ViewModel
//        {
//            get;
//        }

//        private List<Product> Products = new List<Product>();

//        public ProductPage()
//        {
//            ViewModel = App.GetService<ProductViewModel>();
//            InitializeComponent();
//            LoadProductsAsync();
//        }

//        private async Task LoadProductsAsync()
//        {
//            var productDataService = App.GetService<IProductDataService>();
//            Products = (await productDataService.LoadDataAsync()).ToList();
//        }

//        // Handle text change and present suitable items
//        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
//        {
//            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
//            {
//                var suitableItems = new List<string>();
//                var splitText = sender.Text.ToLower().Split(" ");
//                foreach (var product in Products)
//                {
//                    var found = splitText.All((key) =>
//                    {
//                        return product.Name.ToLower().Contains(key) || product.Id.ToLower().Contains(key);
//                    });
//                    if (found)
//                    {
//                        suitableItems.Add($"{product.Name} (ID: {product.Id})");
//                    }
//                }
//                if (suitableItems.Count == 0)
//                {
//                    suitableItems.Add("No results found");
//                }
//                sender.ItemsSource = suitableItems;
//            }
//            else if (string.IsNullOrEmpty(sender.Text))
//            {
//                ViewModel.Source.Clear();
//                foreach (var product in Products)
//                {
//                    ViewModel.Source.Add(product);
//                }
//            }
//        }

//        // Handle user selecting an item, in our case just output the selected item.
//        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
//        {
//            SuggestionOutput.Text = args.SelectedItem.ToString();
//            var selectedProduct = Products.FirstOrDefault(p => $"{p.Name} (ID: {p.Id})" == args.SelectedItem.ToString());
//            if (selectedProduct != null)
//            {
//                ViewModel.Source.Clear();
//                ViewModel.Source.Add(selectedProduct);
//            }
//            else
//            {
//                // Reset to show all products if no specific product is selected
//                ViewModel.Source.Clear();
//                foreach (var product in Products)
//                {
//                    ViewModel.Source.Add(product);
//                }
//            }
//        }
//    }
//}


using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;
using Sale_Project.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale_Project.Views;

public sealed partial class ProductPage : Page
{
    public ProductViewModel ViewModel
    {
        get;
    }

    private List<Product> Products = new();

    public ProductPage()
    {
        ViewModel = App.GetService<ProductViewModel>();
        InitializeComponent();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        var productDataService = App.GetService<IProductDataService>();
        Products = (await productDataService.LoadDataAsync()).ToList();
        ViewModel.Source.Clear();
        foreach (var product in Products)
        {
            ViewModel.Source.Add(product);
        }
    }

    // Handle text change and present suitable items
    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var product in Products)
            {
                var found = splitText.All((key) =>
                {
                    return product.Name.ToLower().Contains(key) || product.Id.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add($"{product.Name} (ID: {product.Id})");
                }
            }

            if (string.IsNullOrEmpty(sender.Text))
            {
                ViewModel.Source.Clear();
                foreach (var product in Products)
                {
                    ViewModel.Source.Add(product);
                }
            }

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }
        
    }

    // Handle user selecting an item, in our case just output the selected item.
    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        SuggestionOutput.Text = args.SelectedItem.ToString();
        var selectedProduct = Products.FirstOrDefault(p => $"{p.Name} (ID: {p.Id})" == args.SelectedItem.ToString());
        if (selectedProduct != null)
        {
            ViewModel.Source.Clear();
            ViewModel.Source.Add(selectedProduct);
        }
        else
        {
            // Reset to show all products if no specific product is selected
            ViewModel.Source.Clear();
            foreach (var product in Products)
            {
                ViewModel.Source.Add(product);
            }
        }
    }

    private async void ShowAddProductDialog_Click(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Thêm hàng";
        dialog.PrimaryButtonText = "Lưu";
        //dialog.SecondaryButtonText = "Don't Save";
        dialog.CloseButtonText = "Bỏ qua";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Content = new AddProductDialog();

        var result = await dialog.ShowAsync();
    }
}