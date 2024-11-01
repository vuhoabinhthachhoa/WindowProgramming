using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Sale_Project.Services;
using Sale_Project.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProductUpdatePage : Page
{
    public class ProductUpdatePageViewModel
    {
        IDao _dao;
        public ProductUpdatePageViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public Product Info { get; set; } = new Product();

        public bool Update()
        {
            return _dao.UpdateProduct(Info);
        }
    }

    public ProductUpdatePageViewModel ViewModel { get; set; } = new ProductUpdatePageViewModel();

    public ProductUpdatePage()
    {
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.Info = e.Parameter as Product;

        base.OnNavigatedTo(e);
    }

    private async void submitButton_Click(object sender, RoutedEventArgs e)
    {
        bool success = ViewModel.Update();

        if (success)
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Update  Product",
                Content = "Successfully updated Product:" + ViewModel.Info.Name,
                CloseButtonText = "OK"
            }.ShowAsync();
            Frame.GoBack();
        }
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}
