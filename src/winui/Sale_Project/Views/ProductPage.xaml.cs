using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class ProductPage : Page
{
    public ProductViewModel ViewModel
    {
        get;
    }

    public ProductPage()
    {
        ViewModel = App.GetService<ProductViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void AddProductButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.AddProduct();
    }
    private async void SearchProductButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchProduct();
    }

    private async void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToPreviousPage();
    }

    private async void NextButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToNextPage();
    }

    private async void SortByIDAscButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortByIDAsc();
    }

    private async void SortByIDDescButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortByIDDesc();
    }

    //private async void BusinessStatusChanged(object sender, DataContextChangedEventArgs e)
    //{
    //    await ViewModel.BusinessStatusChanged();
    //}
}
