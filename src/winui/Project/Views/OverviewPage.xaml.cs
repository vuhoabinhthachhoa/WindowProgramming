using Microsoft.UI.Xaml.Controls;

using Project.ViewModels;

namespace Project.Views;

public sealed partial class OverviewPage : Page
{
    public OverviewViewModel ViewModel
    {
        get;
    }

    public OverviewPage()
    {
        ViewModel = App.GetService<OverviewViewModel>();
        InitializeComponent();
    }
}
