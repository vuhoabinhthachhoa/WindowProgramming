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

    private void addProductButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ProductAddPage));
     
    }

    private async void deleteProductButton_Click(object sender, RoutedEventArgs e)
    {
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
        var product = itemsDataGrid.SelectedItem as Product;

        Frame.Navigate(typeof(ProductUpdatePage), product);
    }
}

