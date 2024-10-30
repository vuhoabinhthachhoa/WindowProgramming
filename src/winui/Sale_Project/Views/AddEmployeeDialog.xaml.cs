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
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;
using Sale_Project.Core.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sale_Project.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddEmployeeDialog : Page
    {
        public AddEmployeeDialog()
        {
            this.InitializeComponent();
        }

        public bool ValidateInput()
        {
            if (Id_Input.Text == "" || Phonenumber_Input.Text == "" || Name_Input.Text == "")
            {
                Warning.Text = "Please fill all the fields";
                return false;
            }
            return true;
        }

        public Employee GetEmployee()
        {
            return new Employee()
            {
                Id = Id_Input.Text,
                Email = Email_Input.Text,
                Name = Name_Input.Text,
                Phonenumber = Phonenumber_Input.Text,
                Address = Address_Input.Text
            };
        }
    }
}