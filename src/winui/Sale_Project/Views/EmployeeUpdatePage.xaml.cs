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
using Sale_Project.Contracts.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EmployeeUpdatePage : Page
{

    public EmployeeUpdatePageViewModel ViewModel { get; set; }

    public EmployeeUpdatePage()
    {
        ViewModel = new EmployeeUpdatePageViewModel();
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.Info = e.Parameter as Employee;

        base.OnNavigatedTo(e);
    }

    private async void submitButton_Click(object sender, RoutedEventArgs e)
    {
        var (success, message) = ViewModel.UpdateEmployee();

        if (success)
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Update Employee",
                Content = "Successfully updated Employee:" + ViewModel.Info.Name,
                CloseButtonText = "OK"
            }.ShowAsync();
            Frame.GoBack();
        }
        else
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Update employee",
                Content = message,
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}