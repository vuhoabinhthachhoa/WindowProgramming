using Sale_Project.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Sale_Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Sale_Project.Core.Models;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CustomerAddPage : Page
{
    public class CustomerAddPageViewModel
    {
        public CustomerAddPageViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public Customer Info { get; set; } = new Customer();
        IDao _dao;

        public bool AddCustomer()
        {
            //Info.Images = "/Assets/avatar07.jpg";
            return _dao.AddCustomer(Info);
        }
    }

    public CustomerAddPageViewModel ViewModel { get; set; } = new CustomerAddPageViewModel();

    public CustomerAddPage()
    {
        this.InitializeComponent();
    }

    private async void submitButton_Click(object sender, RoutedEventArgs e)
    {
        bool success = ViewModel.AddCustomer();

        if (success)
        {

            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Insert new employee",
                Content = "Successfully inserted employee:" + ViewModel.Info.Name,
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