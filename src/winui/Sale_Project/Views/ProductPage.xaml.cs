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
using System.Reflection;
using System.Reflection.Metadata;
using Sale_Project.Core.Services;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.WinUI.UI.Controls;


namespace Sale_Project.Views;
public sealed partial class ProductPage : Page
{
    public ProductViewModel ViewModel 
    {
        get; set;
    }
    public ProductPage()
    {
        this.InitializeComponent();
        ViewModel = new ProductViewModel();
        // DataContext = this;
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var product in ViewModel.Products)
            {
                var found = splitText.All((key) =>
                {
                    return product.Name.ToLower().Contains(key);
                });
                if (found)
                {
                    //suitableItems.Add($"{product.Name} (ID: {product.ID})");
                    suitableItems.Add($"{product.Name}");
                }
            }


            //if (string.IsNullOrEmpty(sender.Text))

            {
                ViewModel.Products.Clear();
                foreach (var product in ViewModel.Products)
                {
                    ViewModel.Products.Add(product);
                }
            }

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }
        ViewModel.Search();

    }

    private void addProductButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ProductAddPage));
     
    }

    private async void deleteProductButton_Click(object sender, RoutedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem == null)
        {
            return;
        }

        var product = itemsDataGrid.SelectedItem as Product;
        bool success = ViewModel.Remove(product);
         
        if (success)
        {
            await new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Delete",
                Content = "Delete successfully",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
        else
        {
            await new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Delete",
                Content = "Delete failed",
                CloseButtonText = "Cannot delete product with id: " + product.ID
            }.ShowAsync();
        }
    }

    private void updateProductButton_Click(object sender, RoutedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem == null)
        {
            return;
        }

        var product = itemsDataGrid.SelectedItem as Product;

        Frame.Navigate(typeof(ProductUpdatePage), product);
    }

    private void previousButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoToPreviousPage();
    }

    private void nextButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoToNextPage();
    }

    private void itemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem != null)
        {
            deleteProductButton.IsEnabled = true;
            updateProductButton.IsEnabled = true;
        }
        else
        {
            deleteProductButton.IsEnabled = false;
            updateProductButton.IsEnabled = false;
        }
    }

    private void dataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        if (e.Column.Tag != null)
        {
            string sortColumn = e.Column.Tag.ToString();
            bool ascending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;

            switch (sortColumn)
            {
                case "ID":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.ID ascending select item
                        : from item in ViewModel.Products orderby item.ID descending select item);
                    break;
                case "Name":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.Name ascending select item
                        : from item in ViewModel.Products orderby item.Name descending select item);
                    break;
                case "CategoryID":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.CategoryID ascending select item
                        : from item in ViewModel.Products orderby item.CategoryID descending select item);
                    break;
                case "ImportPrice":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.ImportPrice ascending select item
                        : from item in ViewModel.Products orderby item.ImportPrice descending select item);
                    break;
                case "SellingPrice":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.SellingPrice ascending select item
                        : from item in ViewModel.Products orderby item.SellingPrice descending select item);
                    break;
                case "BranchID":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.BranchID ascending select item
                        : from item in ViewModel.Products orderby item.BranchID descending select item);
                    break;
                case "InventoryQuantity":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.InventoryQuantity ascending select item
                        : from item in ViewModel.Products orderby item.InventoryQuantity descending select item);
                    break;
                case "BusinessStatus":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.BusinessStatus ascending select item
                        : from item in ViewModel.Products orderby item.BusinessStatus descending select item);
                    break;
                case "Size":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.Size ascending select item
                        : from item in ViewModel.Products orderby item.Size descending select item);
                    break;
                case "DiscountPercent":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Product>(ascending
                        ? from item in ViewModel.Products orderby item.DiscountPercent ascending select item
                        : from item in ViewModel.Products orderby item.DiscountPercent descending select item);
                    break;
                default:
                    break;
            }

            e.Column.SortDirection = ascending ? DataGridSortDirection.Ascending : DataGridSortDirection.Descending;

            // Remove sorting indicators from other columns
            foreach (var dgColumn in itemsDataGrid.Columns)
            {
                if (dgColumn.Tag != null && dgColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    dgColumn.SortDirection = null;
                }
            }
        }
    }
}

