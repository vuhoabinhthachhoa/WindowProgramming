using Microsoft.UI.Xaml.Controls;

using Project.ViewModels;

namespace Project.Views;

public sealed partial class ProductCategoryPage : Page
{
    public ProductCategoryViewModel ViewModel
    {
        get;
    }

    public ProductCategoryPage()
    {
        ViewModel = App.GetService<ProductCategoryViewModel>();
        InitializeComponent();
    }
}
