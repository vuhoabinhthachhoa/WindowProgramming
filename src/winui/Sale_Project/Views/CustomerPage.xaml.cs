using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

public sealed partial class CustomerPage : Page
{
    public CustomerViewModel ViewModel
    {
        get;
    }

    public CustomerPage()
    {
        ViewModel = App.GetService<CustomerViewModel>();
        InitializeComponent();
    }
}
