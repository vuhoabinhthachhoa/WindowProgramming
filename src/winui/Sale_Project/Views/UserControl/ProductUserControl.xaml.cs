using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Sale_Project.Core.Models.Products;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project;
public sealed partial class ProductUserControl : UserControl
{
    private readonly string[] Size = new string[] { "S", "M", "L", "XL" };
    private readonly bool[] BusinessStatus = new bool[] { true, false };
  
    public static readonly DependencyProperty IsBranchNameReadOnlyProperty
        = DependencyProperty.Register(
            "IsBranchNameReadOnly", typeof(string),
            typeof(ProductUserControl),
            new PropertyMetadata(null)
    );

    public string IsBranchNameReadOnly
    {
        get => (string)GetValue(IsBranchNameReadOnlyProperty);
        set => SetValue(IsBranchNameReadOnlyProperty, value);
    }
    public static readonly DependencyProperty InfoProperty
        = DependencyProperty.Register(
            "Info", typeof(Product),
            typeof(ProductUserControl),
            new PropertyMetadata(null)
    );

    public Product Info
    {
        get => (Product)GetValue(InfoProperty);
        set => SetValue(InfoProperty, value);
    }

    public static readonly DependencyProperty FileStreamProperty
        = DependencyProperty.Register(
            "FileStream", typeof(Stream),
            typeof(ProductUserControl),
            new PropertyMetadata(null)
    );
    public static readonly DependencyProperty IsCategoryIDReadOnlyProperty
        = DependencyProperty.Register(
            "IsCategoryIDReadOnly", typeof(string),
            typeof(ProductUserControl),
            new PropertyMetadata(null)
    );

    public Stream FileStream
    {
        get => (Stream)GetValue(FileStreamProperty);
        set => SetValue(FileStreamProperty, value);
    }

    public string IsCategoryIDReadOnly
    {
        get => (string)GetValue(IsCategoryIDReadOnlyProperty);
        set => SetValue(IsCategoryIDReadOnlyProperty, value);
    }

    public ProductUserControl()
    {
        this.InitializeComponent();
    }

    public async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        PickAPhotoOutputTextBlock.Text = "";

        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        var file = await openPicker.PickSingleFileAsync();

        if (file != null)
        {
            PickAPhotoOutputTextBlock.Text = "Picked photo: " + file.Name;

            try
            {
                var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                //PickAPhotoOutputTextBlock.Text += "\nStream type: " + fileStream.GetType().FullName;

                // Explicitly convert IRandomAccessStream to Stream
                FileStream = fileStream.AsStreamForRead();
            }
            catch (Exception ex)
            {
                PickAPhotoOutputTextBlock.Text = "Error opening file: " + ex.Message;
                FileStream = Stream.Null;
            }
        }
        else
        {
            PickAPhotoOutputTextBlock.Text = "Operation cancelled.";
            FileStream = Stream.Null;
        }
    }

}
