using Microsoft.UI.Xaml.Controls;

using Project.ViewModels;

namespace Project.Views;

public sealed partial class ProductPricingPage : Page
{
    public ProductPricingViewModel ViewModel
    {
        get;
    }

    public ProductPricingPage()
    {
        ViewModel = App.GetService<ProductPricingViewModel>();
        InitializeComponent();
    }
}
