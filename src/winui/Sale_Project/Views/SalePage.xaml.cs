using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

public sealed partial class SalePage : Page
{
    public SaleViewModel ViewModel
    {
        get;
    }

    public SalePage()
    {
        ViewModel = App.GetService<SaleViewModel>();
        InitializeComponent();
    }
}
