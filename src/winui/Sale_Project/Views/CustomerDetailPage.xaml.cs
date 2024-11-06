using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Sale_Project.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using Sale_Project.ViewModels;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CustomerDetailPage : Page
{
    public SampleCustomerDataType SelectedCustomer
    {
        get; set;
    } = new SampleCustomerDataType();

    public CustomerDetailViewModel ViewModel
    {
        get;
    }

    public CustomerDetailPage()
    {
        ViewModel = App.GetService<CustomerDetailViewModel>();
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is SampleCustomerDataType customer)
        {
            SelectedCustomer = customer;

            IdTextBlock.Text = SelectedCustomer.Id;
            NameTextBlock.Text = SelectedCustomer.FirstName + " " + SelectedCustomer.FamilyName;
            EmailTextBlock.Text = SelectedCustomer.Email;
            PhoneTextBlock.Text = SelectedCustomer.Phone;
            AddressTextBlock.Text = SelectedCustomer.Address;
            CityTextBlock.Text = SelectedCustomer.City;
            StateTextBlock.Text = SelectedCustomer.State;
            ZipTextBlock.Text = SelectedCustomer.Zip;
            //VoucherTextBlock.Text = SelectedCustomer.Voucher;
        }
    }

    private void SelectorTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedTab = (sender as TabView)?.SelectedItem as TabViewItem;

        if (selectedTab != null)
        {
            if (selectedTab.Header.ToString() == "Thông tin")
            {
            }
            else if (selectedTab.Header.ToString() == "Voucher")
            {
            }
        }
    }

    private void ManButton_Checked(object sender, RoutedEventArgs e)
    {
        // Add your event handling code here
    }

    private void WomanButton_Checked(object sender, RoutedEventArgs e)
    {
        // Add your event handling code here
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Add your event handling code here
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Add your event handling code here
    }

    private async void UploadImage_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker();
        picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        picker.ViewMode = PickerViewMode.Thumbnail;

        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(picker, hwnd);

        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");

        StorageFile file = await picker.PickSingleFileAsync();
        if (file != null)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            using (stream)
            {
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(stream);
                UploadedImage.Source = bitmapImage;
            }
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (Frame.CanGoBack)
        {
            Frame.GoBack();
        }
    }

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        
    }
}
