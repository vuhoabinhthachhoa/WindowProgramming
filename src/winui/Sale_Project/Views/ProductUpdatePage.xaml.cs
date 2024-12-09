using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;
using Windows.Storage.Pickers;

namespace Sale_Project.Views;

public sealed partial class ProductUpdatePage : Page
{
    public ProductUpdateViewModel ViewModel
    {
        get;
    }

    public ProductUpdatePage()
    {
        ViewModel = App.GetService<ProductUpdateViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }


    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoBack();
    }

    private async void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.UpdateProduct();
    }

    private async void MarkInactiveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkProductInactive();
    }

    private async void MarkActiveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkProductActive();
    }

    public async void UpdateAPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.UpdateAPhoto();
    }
}

