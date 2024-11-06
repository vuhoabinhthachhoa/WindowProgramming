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
using System.ComponentModel;
using Sale_Project.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project;
public sealed partial class EmployeeUserControl : UserControl, INotifyPropertyChanged
{
    private readonly bool[] EmployeeStatus = new bool[] { true, false };
    private readonly string[] JobTitle = new string[] { "Manager", "Salesperson", "Cashier", "Stock Clerk", "Customer Service Representative" };

    private string _warning;
    public string Warning_msg
    {
        get => _warning;
        set
        {
            _warning = value;
            OnPropertyChanged(nameof(Warning));
        }
    }

    public static readonly DependencyProperty InfoProperty
        = DependencyProperty.Register(
            "Info", typeof(Employee),
            typeof(EmployeeUserControl),
            new PropertyMetadata(null, OnInfoChanged)
    );

    public Employee Info
    {
        get => (Employee)GetValue(InfoProperty);
        set
        {
            if (value == null || !int.TryParse(value.ID.ToString(), out _))
            {
                Warning_msg = "The value of ID must be a number.";
                return; // Prevent setting the value if invalid
            }
            SetValue(InfoProperty, value);
            Warning_msg = string.Empty;
        }
    }

    private static void OnInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as EmployeeUserControl;
        if (control != null && e.NewValue is Employee newEmployee)
        {
            if (!int.TryParse(newEmployee.ID.ToString(), out _))
            {
                control.Warning_msg = "The value of ID must be a number.";
                control.SetValue(InfoProperty, e.OldValue); // Revert to the old value
            }
            else
            {
                control.Warning_msg = string.Empty;
            }
        }
    }

    public EmployeeUserControl()
    {
        this.InitializeComponent();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
