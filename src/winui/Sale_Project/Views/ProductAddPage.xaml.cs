using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models.Product;
using Sale_Project.ViewModels;
using Windows.Storage.Pickers;

namespace Sale_Project.Views;

public sealed partial class ProductAddPage : Page
{
    public ProductAddViewModel ViewModel
    {
        get;
    }

    public ProductAddPage()
    {
        ViewModel = App.GetService<ProductAddViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoBack();
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.AddProduct();
    }
  
    public async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.PickAPhoto();
    }
}
