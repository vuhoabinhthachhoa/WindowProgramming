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
    public sealed partial class AddProductDialog : Page
    {
        public AddProductDialog()
        {
            this.InitializeComponent();
        }

        public bool ValidateInput()
        {
            if (Id_Input.Text == "" || Code_Input.Text == "" || Name_Input.Text == "" || Category_id_Input.Text == "" || Import_price_Input.Text == "" || Selling_price_Input.Text == "" || Inventory_quantity_Input.Text == "")
            {
                Warning.Text = "Please fill all the fields";
                return false;
            }
            return true;
        }

        public Product GetProduct()
        {
            return new Product()
            {
                Id = Id_Input.Text,
                Code = Code_Input.Text,
                Name = Name_Input.Text,
                Category_id = Category_id_Input.Text,
                Import_price = Int32.Parse(Import_price_Input.Text),
                Selling_price = Int32.Parse(Selling_price_Input.Text),
                Branch_id = Branch_id_Input.Text,
                Inventory_quantity = Int32.Parse(Inventory_quantity_Input.Text),
                Images = "Image 999",
                Business_status = Business_status_Input.Text,
                Size = Size_Input.Text
            };
        }
    }
}
