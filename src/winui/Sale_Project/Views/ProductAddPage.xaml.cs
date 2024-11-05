using Sale_Project.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Sale_Project.Core.Models;
using Sale_Project.Contracts.Services;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProductAddPage : Page
{
    public class ProductAddPageViewModel
    {
        public ProductAddPageViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IProductDao)) as IProductDao;
        }
        public Product Info { get; set; } = new Product();
        IProductDao _dao;

        public (bool, string) AddProduct()
        {
            return _dao.AddProduct(Info);
            //string message = result ? "Product added successfully." : "Failed to add employee.";
            //return (result, message);
        }
    }

    public ProductAddPageViewModel ViewModel { get; set; } = new ProductAddPageViewModel();

    public ProductAddPage()
    {
        this.InitializeComponent();
    }



    private async void submitButton_Click(object sender, RoutedEventArgs e)
    {
        var (success, message) = ViewModel.AddProduct();

        if (success)
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Insert new product",
                Content = "Successfully inserted product: " + ViewModel.Info.Name,
                CloseButtonText = "OK"
            }.ShowAsync();
            Frame.GoBack();
        }
        else
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Insert new product",
                Content = message,
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}