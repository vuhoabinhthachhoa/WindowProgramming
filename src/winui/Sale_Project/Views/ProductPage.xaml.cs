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
using System.Text.Json;
using Newtonsoft.Json;
using Sale_Project.Core.Helpers;
namespace Sale_Project.Views;
using System.Reflection;
using System.Reflection.Metadata;
using Sale_Project.Core.Services;

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

    //private async void ShowAddProductDialog_Click(object sender, RoutedEventArgs e)
    //{
    //    ContentDialog dialog = new ContentDialog();

    //    // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
    //    dialog.XamlRoot = this.XamlRoot;
    //    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
    //    dialog.Title = "Thêm hàng";
    //    dialog.PrimaryButtonText = "Lưu";
    //    //dialog.SecondaryButtonText = "Don't Save";
    //    dialog.CloseButtonText = "Bỏ qua";
    //    dialog.DefaultButton = ContentDialogButton.Primary;
    //    dialog.Content = new AddProductDialog();
    //    dialog.
    //    var result = await dialog.ShowAsync();
    //}

    private async void ShowAddProductDialog_Click(object sender, RoutedEventArgs e)
    {
        var addProductDialog = new AddProductDialog(); // Declare addProductDialog here

        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Thêm hàng",
            PrimaryButtonText = "Lưu",
            CloseButtonText = "Bỏ qua",
            DefaultButton = ContentDialogButton.Primary,
            Content = addProductDialog
        };

        dialog.PrimaryButtonClick += (s, args) => AddProductDialog_PrimaryButtonClick(s, args, addProductDialog); // Pass addProductDialog to the event handler
        var result = await dialog.ShowAsync();
    }

    private async void AddProductDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args, AddProductDialog addProductDialog)
    {
        if (!addProductDialog.ValidateInput())
        {
            args.Cancel = true;
            return;
        }
        string path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\products.json");

        var json = File.ReadAllText(path);
        var list = JsonConvert.DeserializeObject<List<Product>>(json);
        Product product = addProductDialog.GetProduct(); // Use addProductDialog here
        //list.Add(new Product()
        //{
        //    Id = "999",
        //    Code = "SP099",
        //    Name = "Product 999",
        //    Category_id = "999",
        //    Import_price = 3000,
        //    Selling_price = 4000,
        //    Branch_id = "999",
        //    Inventory_quantity = 30,
        //    Images = "Image 999",
        //    Business_status = "Active",
        //    Size = "L",
        //    Discount_percent = 0.3
        //});

        list.Add(product); // Add the product from the dialog

        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        // Update ViewModel Source from the updated JSON file
        ViewModel.Source.Clear();
        json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        foreach (var item in Products)
        {
            ViewModel.Source.Add(item);
        }
    }
    

    private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
    {
        var product = itemsDataGrid.SelectedIndex;
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\products.json");

        var json = File.ReadAllText(path);
        var list = JsonConvert.DeserializeObject<List<Product>>(json);
        list.RemoveAt(product);

        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        // Update ViewModel Source from the updated JSON file
        ViewModel.Source.Clear();
        json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        foreach (var item in Products)
        {
            ViewModel.Source.Add(item);
        }
    }


}