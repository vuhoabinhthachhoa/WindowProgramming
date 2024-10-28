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
        // public async void AddProductDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        // {
        //     string path = Path.Combine(
        //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        //@"..\..\..\..\..\..\MockData\products.json");


        //     var json = File.ReadAllText(path);
        //     var list = JsonConvert.DeserializeObject<List<Product>>(json);
        //     list.Add(new Product()
        //     {
        //         Id = "999",
        //         Code = "SP099",
        //         Name = "Product 999",
        //         Category_id = "999",
        //         Import_price = 3000,
        //         Selling_price = 4000,
        //         Branch_id = "999",
        //         Inventory_quantity = 30,
        //         Images = "Image 999",
        //         Business_status = "Active",
        //         Size = "L",
        //         Discount_percent = 0.3
        //     });

        //     var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        //     File.WriteAllText(path, convertedJson);


        // }
    }
}
