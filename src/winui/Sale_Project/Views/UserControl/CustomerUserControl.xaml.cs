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
public sealed partial class CustomerUserControl : UserControl
{
    public static readonly DependencyProperty InfoProperty
        = DependencyProperty.Register(
            "Info", typeof(Customer),
            typeof(CustomerUserControl),
            new PropertyMetadata(null)
    );

    public Customer Info
    {
        get => (Customer)GetValue(InfoProperty);
        set => SetValue(InfoProperty, value);
    }


    public CustomerUserControl()
    {
        this.InitializeComponent();
    }
}