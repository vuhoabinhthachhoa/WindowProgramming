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
using Sale_Project.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project;
public sealed partial class ProductUserControl : UserControl
{
    private readonly string[] Size = new string[] { "S", "M", "L", "XL" };
    private readonly string[] BusinessStatus = new string[] { "Đang kinh doanh", "Ngừng kinh doanh" };

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


    public ProductUserControl()
    {
        this.InitializeComponent();
    }
}
