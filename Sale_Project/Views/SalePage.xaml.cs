using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents; 
using Sale_Project.ViewModels;
using Sale_Project.Core.Models;

namespace Sale_Project.Views;

public sealed partial class SalePage : Page
{
    public SaleViewModel ViewModel
    {
        get;
    }

    public SalePage()
    {
        ViewModel = App.GetService<SaleViewModel>();
        InitializeComponent();
    }

    private readonly List<SampleProductDataType> Products = new()
    {
        new () {Product_Name="Áo vest nam màu xanh", Product_id="NAM003", Product_Amount=1, Product_Price=3699000, Product_discount=0.8 },
        new () {Product_Name="Ví màu xanh nhạt", Product_id="NU006", Product_Amount=4, Product_Price = 1899000, Product_discount = 0.2 },
        new () {Product_Name="Giày da Sanvado màu đen", Product_id = "NAM021", Product_Amount=0, Product_Price = 539100, Product_discount=0.9 },
        new () {Product_Name="Quần kaki nam màu nâu", Product_id = "NAM018", Product_Amount=2, Product_Price = 599500, Product_discount=0.95 },
        new () {Product_Name="Cà vạt nam đồng giá 95k", Product_id = "NAM009", Product_Amount=10, Product_Price = 95000, Product_discount=1.0 },
    };

    private readonly List<string> selectedProducts = new ();

    private void AutoSuggestBox_TextChangedProduct(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = Products
                .Where(e => e.Product_Name.StartsWith(sender.Text, StringComparison.OrdinalIgnoreCase))
                .Select(e => $"{e.Product_Name} \n Mã sản phẩm: {e.Product_id}       Tồn kho: {e.Product_Amount}" +
                $"       Giá: {e.Product_Price:N0} VND   Giảm giá: {e.Product_discount:P0}  ")
                .ToList(); ;

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }

            sender.ItemsSource = suitableItems;
        }
    }

    private void AutoSuggestBox_SuggestionChosenProduct(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var selectedItem = args.SelectedItem?.ToString(); 

        if (!string.IsNullOrEmpty(selectedItem) && !selectedProducts.Contains(selectedItem))
        {
            selectedProducts.Add("\n" + selectedItem);
            UpdateSelectedItemsDisplay();
        }
    }

    private void UpdateSelectedItemsDisplay()
    {
        SelectedItemsDisplayProduct.Blocks.Clear();
        foreach (var item in selectedProducts)
        {
            var paragraph = new Paragraph();
            var run = new Run { Text = item };

            var deleteButton = new Button
            {
                Content = "X",
                Margin = new Thickness(10, 5, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                Tag = item
            };
            deleteButton.Click += DeleteButton_Click;

            var inlineUIContainer = new InlineUIContainer
            {
                Child = deleteButton
            };

            paragraph.Inlines.Add(run);
            paragraph.Inlines.Add(inlineUIContainer);

            SelectedItemsDisplayProduct.Blocks.Add(paragraph);
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button deleteButton && deleteButton.Tag is string itemToDelete)
        {
            selectedProducts.Remove(itemToDelete);
            UpdateSelectedItemsDisplay();
        }
    }

    private readonly List<SampleCustomerDataType> _emailSamples = new()
   {
       new () { Id = "1", FirstName = "Marcus", FamilyName = "Perryman", Email = "marcus.perryman@example.com", Phone = "123-456-7890", Address = "123 Elm St", City = "Springfield", State = "IL", Zip = "62701", Voucher = "VOUCHER1" },
       new () { Id = "2", FirstName = "Michael", FamilyName = "Hawker", Email = "michael.hawker@example.com", Phone = "234-567-8901", Address = "234 Oak St", City = "Springfield", State = "IL", Zip = "62702", Voucher = "VOUCHER2" },
       new () { Id = "3", FirstName = "Matt", FamilyName = "Lacey", Email = "matt.lacey@example.com", Phone = "345-678-9012", Address = "345 Pine St", City = "Springfield", State = "IL", Zip = "62703", Voucher = "VOUCHER3" },
       new () { Id = "4", FirstName = "Alexandre", FamilyName = "Chohfi", Email = "alexandre.chohfi@example.com", Phone = "456-789-0123", Address = "456 Maple St", City = "Springfield", State = "IL", Zip = "62704", Voucher = "VOUCHER4" },
       new () { Id = "5", FirstName = "Filip", FamilyName = "Wallberg", Email = "filip.wallberg@example.com", Phone = "567-890-1234", Address = "567 Birch St", City = "Springfield", State = "IL", Zip = "62705", Voucher = "VOUCHER5" },
       new () { Id = "6", FirstName = "Shane", FamilyName = "Weaver", Email = "shane.weaver@example.com", Phone = "678-901-2345", Address = "678 Cedar St", City = "Springfield", State = "IL", Zip = "62706", Voucher = "VOUCHER6" },
       new () { Id = "7", FirstName = "Vincent", FamilyName = "Gromfeld", Email = "vincent.gromfeld@example.com", Phone = "789-012-3456", Address = "789 Walnut St", City = "Springfield", State = "IL", Zip = "62707", Voucher = "VOUCHER7" },
       new () { Id = "8", FirstName = "Sergio", FamilyName = "Pedri", Email = "sergio.pedri@example.com", Phone = "890-123-4567", Address = "890 Chestnut St", City = "Springfield", State = "IL", Zip = "62708", Voucher = "VOUCHER8" },
       new () { Id = "9", FirstName = "Alex", FamilyName = "Wilber", Email = "alex.wilber@example.com", Phone = "901-234-5678", Address = "901 Poplar St", City = "Springfield", State = "IL", Zip = "62709", Voucher = "VOUCHER9" },
       new () { Id = "10", FirstName = "Allan", FamilyName = "Deyoung", Email = "allan.deyoung@example.com", Phone = "012-345-6789", Address = "1010 Spruce St", City = "Springfield", State = "IL", Zip = "62710", Voucher = "VOUCHER10" },
       new () { Id = "11", FirstName = "Adele", FamilyName = "Vance", Email = "adele.vance@example.com", Phone = "123-456-7891", Address = "1111 Fir St", City = "Springfield", State = "IL", Zip = "62711", Voucher = "VOUCHER11" },
       new () { Id = "12", FirstName = "Grady", FamilyName = "Archie", Email = "grady.archie@example.com", Phone = "234-567-8902", Address = "1212 Ash St", City = "Springfield", State = "IL", Zip = "62712", Voucher = "VOUCHER12" },
       new () { Id = "13", FirstName = "Megan", FamilyName = "Bowen", Email = "megan.bowen@example.com", Phone = "345-678-9013", Address = "1313 Sycamore St", City = "Springfield", State = "IL", Zip = "62713", Voucher = "VOUCHER13" },
       new () { Id = "14", FirstName = "Ben", FamilyName = "Walters", Email = "ben.walters@example.com", Phone = "456-789-0124", Address = "1414 Willow St", City = "Springfield", State = "IL", Zip = "62714", Voucher = "VOUCHER14" },
       new () { Id = "15", FirstName = "Debra", FamilyName = "Berger", Email = "debra.berger@example.com", Phone = "567-890-1235", Address = "1515 Larch St", City = "Springfield", State = "IL", Zip = "62715", Voucher = "VOUCHER15" },
       new () { Id = "16", FirstName = "Emily", FamilyName = "Braun", Email = "emily.braun@example.com", Phone = "678-901-2346", Address = "1616 Elm St", City = "Springfield", State = "IL", Zip = "62716", Voucher = "VOUCHER16" },
       new () { Id = "17", FirstName = "Christine", FamilyName = "Cline", Email = "christine.cline@example.com", Phone = "789-012-3457", Address = "1717 Oak St", City = "Springfield", State = "IL", Zip = "62717", Voucher = "VOUCHER17" },
       new () { Id = "18", FirstName = "Enrico", FamilyName = "Catteneo", Email = "enrico.catteneo@example.com", Phone = "890-123-4568", Address = "1818 Pine St", City = "Springfield", State = "IL", Zip = "62718", Voucher = "VOUCHER18" },
       new () { Id = "19", FirstName = "Davit", FamilyName = "Badalyan", Email = "davit.badalyan@example.com", Phone = "901-234-5679", Address = "1919 Birch St", City = "Springfield", State = "IL", Zip = "62719", Voucher = "VOUCHER19" },
       new () { Id = "20", FirstName = "Diego", FamilyName = "Siciliani", Email = "diego.siciliani@example.com", Phone = "012-345-6780", Address = "2020 Cedar St", City = "Springfield", State = "IL", Zip = "62720", Voucher = "VOUCHER20" },
       new () { Id = "21", FirstName = "Raul", FamilyName = "Razo", Email = "raul.razo@example.com", Phone = "123-456-7892", Address = "2121 Chestnut St", City = "Springfield", State = "IL", Zip = "62721", Voucher = "VOUCHER21" },
       new () { Id = "22", FirstName = "Miriam", FamilyName = "Graham", Email = "miriam.graham@example.com", Phone = "234-567-8903", Address = "2222 Poplar St", City = "Springfield", State = "IL", Zip = "62722", Voucher = "VOUCHER22" },
       new () { Id = "23", FirstName = "Lynne", FamilyName = "Robbins", Email = "lynne.robbins@example.com", Phone = "345-678-9014", Address = "2323 Spruce St", City = "Springfield", State = "IL", Zip = "62723", Voucher = "VOUCHER23" },
       new () { Id = "24", FirstName = "Lydia", FamilyName = "Holloway", Email = "lydia.holloway@example.com", Phone = "456-789-0125", Address = "2424 Fir St", City = "Springfield", State = "IL", Zip = "62724", Voucher = "VOUCHER24" },
       new () { Id = "25", FirstName = "Nestor", FamilyName = "Wilke", Email = "nestor.wilke@example.com", Phone = "567-890-1236", Address = "2525 Ash St", City = "Springfield", State = "IL", Zip = "62725", Voucher = "VOUCHER25" },
       new () { Id = "26", FirstName = "Patti", FamilyName = "Fernandez", Email = "patti.fernandez@example.com", Phone = "678-901-2347", Address = "2626 Sycamore St", City = "Springfield", State = "IL", Zip = "62726", Voucher = "VOUCHER26" },
       new () { Id = "27", FirstName = "Pradeep", FamilyName = "Gupta", Email = "pradeep.gupta@example.com", Phone = "789-012-3458", Address = "2727 Willow St", City = "Springfield", State = "IL", Zip = "62727", Voucher = "VOUCHER27" }
   };

    private void AutoSuggestBox_TextChangedCustomer(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = _emailSamples
                .Where(e => e.FirstName.StartsWith(sender.Text, StringComparison.OrdinalIgnoreCase))
                .Select(e => $"{e.Id} \n{e.FirstName} {e.FamilyName} \n{e.Address} \n{e.State}, {e.City}\n{e.Email}\n{e.Zip}\n{e.Phone}\n")
                .ToList();

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }

            sender.ItemsSource = suitableItems;
        }
    }

    private SampleCustomerDataType? _selectedCustomer = new ();

    private void AutoSuggestBox_SuggestionChosenCustomer(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem.ToString() != "No results found")
        {
            var selectedCustomer = _emailSamples.FirstOrDefault(e =>
                $"{e.Id} \n{e.FirstName} {e.FamilyName} \n{e.Address} \n{e.State}, {e.City}\n{e.Email}\n{e.Zip}\n{e.Phone}\n" == args.SelectedItem.ToString());
            _selectedCustomer = selectedCustomer;
            OpenDetailsButton.IsEnabled = selectedCustomer != null;

            if (selectedCustomer != null)
            {
                NameTextBox.Text = $"{selectedCustomer.FirstName} {selectedCustomer.FamilyName}";
                PhoneTextBox.Text = selectedCustomer.Phone;
                AddressTextBox.Text = selectedCustomer.Address;
                CityTextBox.Text = selectedCustomer.City;
                DistrictTextBox.Text = selectedCustomer.State;
                VoucherTextBox.Text = selectedCustomer.Voucher;
            }

            sender.Text = string.Empty;
        }
    }

    private void OpenDetailsButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedCustomer != null)
        {
            Frame.Navigate(typeof(CustomerDetailPage), _selectedCustomer);
        }
    }

    private void InstructionButton_Click(object sender, RoutedEventArgs e)
    {
        // Handle instruction button click
    }

    private void FeedbackButton_Click(object sender, RoutedEventArgs e)
    {
        // Handle feedback button click
    }
}
