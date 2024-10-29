using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

public sealed partial class AccountPage : Page
{
    public AccountViewModel ViewModel
    {
        get;
    }

    public AccountPage()
    {
        ViewModel = App.GetService<AccountViewModel>();
        InitializeComponent();
    }

    // List of cats
    private readonly List<string> Provinces = new()
    {
        "An Giang",
        "Bà Rịa - Vũng Tàu",
        "Bắc Giang",
        "Bắc Kạn",
        "Bạc Liêu",
        "Bắc Ninh",
        "Bến Tre",
        "Bình Định",
        "Bình Dương",
        "Bình Phước",
        "Bình Thuận",
        "Cà Mau",
        "Cần Thơ",
        "Cao Bằng",
        "Đà Nẵng",
        "Đắk Lắk",
        "Đắk Nông",
        "Điện Biên",
        "Đồng Nai",
        "Đồng Tháp",
        "Gia Lai",
        "Hà Giang",
        "Hà Nam",
        "Hà Nội",
        "Hà Tĩnh",
        "Hải Dương",
        "Hải Phòng",
        "Hậu Giang",
        "Hòa Bình",
        "Hưng Yên",
        "Khánh Hòa",
        "Kiên Giang",
        "Kon Tum",
        "Lai Châu",
        "Lâm Đồng",
        "Lạng Sơn",
        "Lào Cai",
        "Long An",
        "Nam Định",
        "Nghệ An",
        "Ninh Bình",
        "Ninh Thuận",
        "Phú Thọ",
        "Phú Yên",
        "Quảng Bình",
        "Quảng Nam",
        "Quảng Ngãi",
        "Quảng Ninh",
        "Quảng Trị",
        "Sóc Trăng",
        "Sơn La",
        "Tây Ninh",
        "Thái Bình",
        "Thái Nguyên",
        "Thanh Hóa",
        "Thừa Thiên Huế",
        "Tiền Giang",
        "TP. Hồ Chí Minh",
        "Trà Vinh",
        "Tuyên Quang",
        "Vĩnh Long",
        "Vĩnh Phúc",
        "Yên Bái"
    };

    private readonly Dictionary<string, List<string>> Districts = new()
    {
        { "An Giang", new List<string> { "Long Xuyên", "Châu Đốc", "Châu Phú", "Tân Châu" } },
        { "Bà Rịa - Vũng Tàu", new List<string> { "Vũng Tàu", "Bà Rịa", "Châu Đức", "Đất Đỏ" } },
        { "Bắc Giang", new List<string> { "Bắc Giang", "Hiệp Hòa", "Lạng Giang", "Sơn Động" } },
        { "Bắc Kạn", new List<string> { "Bắc Kạn", "Ba Bể", "Ngân Sơn", "Chợ Đồn" } },
        { "Bạc Liêu", new List<string> { "Bạc Liêu", "Hồng Dân", "Giá Rai", "Vĩnh Lợi" } },
        { "Bắc Ninh", new List<string> { "Bắc Ninh", "Tiên Du", "Từ Sơn", "Yên Phong" } },
        { "Bến Tre", new List<string> { "Bến Tre", "Ba Tri", "Châu Thành", "Mỏ Cày Nam" } },
        { "Bình Định", new List<string> { "Quy Nhơn", "Hoài Nhơn", "Tuy Phước", "An Nhơn" } },
        { "Bình Dương", new List<string> { "Thủ Dầu Một", "Bến Cát", "Dĩ An", "Tân Uyên" } },
        { "Bình Phước", new List<string> { "Đồng Xoài", "Phước Long", "Bù Đăng", "Chơn Thành" } },
        { "Bình Thuận", new List<string> { "Phan Thiết", "La Gi", "Tuy Phong", "Bắc Bình" } },
        { "Cà Mau", new List<string> { "Cà Mau", "Năm Căn", "Cái Nước", "Ngọc Hiển" } },
        { "Cần Thơ", new List<string> { "Ninh Kiều", "Bình Thủy", "Cái Răng", "Thốt Nốt" } },
        { "Cao Bằng", new List<string> { "Cao Bằng", "Trùng Khánh", "Hà Quảng", "Nguyên Bình" } },
        { "Đà Nẵng", new List<string> { "Hải Châu", "Thanh Khê", "Liên Chiểu", "Sơn Trà" } },
        { "Đắk Lắk", new List<string> { "Buôn Ma Thuột", "Ea H'leo", "Krông Pắc", "Cư M'gar" } },
        { "Đắk Nông", new List<string> { "Gia Nghĩa", "Đắk Mil", "Cư Jút", "Krông Nô" } },
        { "Điện Biên", new List<string> { "Điện Biên Phủ", "Mường Ảng", "Tủa Chùa", "Mường Chà" } },
        { "Đồng Nai", new List<string> { "Biên Hòa", "Long Khánh", "Long Thành", "Trảng Bom" } },
        { "Đồng Tháp", new List<string> { "Cao Lãnh", "Sa Đéc", "Lấp Vò", "Châu Thành" } },
        { "Gia Lai", new List<string> { "Pleiku", "An Khê", "Ayun Pa", "Chư Sê" } },
        { "Hà Giang", new List<string> { "Hà Giang", "Quản Bạ", "Yên Minh", "Đồng Văn" } },
        { "Hà Nam", new List<string> { "Phủ Lý", "Duy Tiên", "Kim Bảng", "Thanh Liêm" } },
        { "Hà Nội", new List<string> { "Ba Đình", "Hoàn Kiếm", "Tây Hồ", "Long Biên" } },
        { "Hà Tĩnh", new List<string> { "Hà Tĩnh", "Hồng Lĩnh", "Kỳ Anh", "Cẩm Xuyên" } },
        { "Hải Dương", new List<string> { "Hải Dương", "Chí Linh", "Kinh Môn", "Ninh Giang" } },
        { "Hải Phòng", new List<string> { "Hồng Bàng", "Lê Chân", "Ngô Quyền", "Thủy Nguyên" } },
        { "Hậu Giang", new List<string> { "Vị Thanh", "Ngã Bảy", "Châu Thành", "Phụng Hiệp" } },
        { "Hòa Bình", new List<string> { "Hòa Bình", "Lạc Sơn", "Mai Châu", "Cao Phong" } },
        { "Hưng Yên", new List<string> { "Hưng Yên", "Mỹ Hào", "Khoái Châu", "Văn Lâm" } },
        { "Khánh Hòa", new List<string> { "Nha Trang", "Cam Ranh", "Diên Khánh", "Vạn Ninh" } },
        { "Kiên Giang", new List<string> { "Rạch Giá", "Phú Quốc", "Hà Tiên", "Giồng Riềng" } },
        { "Kon Tum", new List<string> { "Kon Tum", "Đắk Hà", "Ngọc Hồi", "Đắk Tô" } },
        { "Lai Châu", new List<string> { "Lai Châu", "Phong Thổ", "Tam Đường", "Tân Uyên" } },
        { "Lâm Đồng", new List<string> { "Đà Lạt", "Bảo Lộc", "Đức Trọng", "Di Linh" } },
        { "Lạng Sơn", new List<string> { "Lạng Sơn", "Cao Lộc", "Chi Lăng", "Bình Gia" } },
        { "Lào Cai", new List<string> { "Lào Cai", "Sa Pa", "Bát Xát", "Bảo Thắng" } },
        { "Long An", new List<string> { "Tân An", "Bến Lức", "Đức Hòa", "Cần Giuộc" } },
        { "Nam Định", new List<string> { "Nam Định", "Giao Thủy", "Hải Hậu", "Ý Yên" } },
        { "Nghệ An", new List<string> { "Vinh", "Cửa Lò", "Quỳnh Lưu", "Con Cuông" } },
        { "Ninh Bình", new List<string> { "Ninh Bình", "Tam Điệp", "Gia Viễn", "Hoa Lư" } },
        { "Ninh Thuận", new List<string> { "Phan Rang - Tháp Chàm", "Ninh Hải", "Ninh Phước", "Thuận Bắc" } },
        { "Phú Thọ", new List<string> { "Việt Trì", "Phú Thọ", "Thanh Sơn", "Đoan Hùng" } },
        { "Phú Yên", new List<string> { "Tuy Hòa", "Sông Cầu", "Đồng Xuân", "Tuy An" } },
        { "Quảng Bình", new List<string> { "Đồng Hới", "Ba Đồn", "Bố Trạch", "Quảng Trạch" } },
        { "Quảng Nam", new List<string> { "Tam Kỳ", "Hội An", "Điện Bàn", "Đại Lộc" } },
        { "Quảng Ngãi", new List<string> { "Quảng Ngãi", "Đức Phổ", "Bình Sơn", "Sơn Tịnh" } },
        { "Quảng Ninh", new List<string> { "Hạ Long", "Cẩm Phả", "Móng Cái", "Uông Bí" } },
        { "Quảng Trị", new List<string> { "Đông Hà", "Quảng Trị", "Cam Lộ", "Gio Linh" } },
        { "Sóc Trăng", new List<string> { "Sóc Trăng", "Vĩnh Châu", "Kế Sách", "Mỹ Xuyên" } },
        { "Sơn La", new List<string> { "Sơn La", "Mộc Châu", "Mai Sơn", "Sông Mã" } },
        { "Tây Ninh", new List<string> { "Tây Ninh", "Hòa Thành", "Trảng Bàng", "Tân Châu" } },
        { "Thái Bình", new List<string> { "Thái Bình", "Quỳnh Phụ", "Tiền Hải", "Kiến Xương" } },
        { "Thái Nguyên", new List<string> { "Thái Nguyên", "Sông Công", "Đại Từ", "Phổ Yên" } },
        { "Thanh Hóa", new List<string> { "Thanh Hóa", "Sầm Sơn", "Bỉm Sơn", "Nghi Sơn" } },
        { "Thừa Thiên Huế", new List<string> { "Huế", "Hương Thủy", "Hương Trà", "Phong Điền" } },
        { "Tiền Giang", new List<string> { "Mỹ Tho", "Gò Công", "Cai Lậy", "Châu Thành" } },
        { "TP. Hồ Chí Minh", new List<string> { "Quận 1", "Quận 2", "Quận 3", "Quận 4" } },
        { "Trà Vinh", new List<string> { "Trà Vinh", "Càng Long", "Cầu Kè", "Tiểu Cần" } },
        { "Tuyên Quang", new List<string> { "Tuyên Quang", "Yên Sơn", "Chiêm Hóa", "Sơn Dương" } },
        { "Vĩnh Long", new List<string> { "Vĩnh Long", "Bình Minh", "Long Hồ", "Tam Bình" } },
        { "Vĩnh Phúc", new List<string> { "Vĩnh Yên", "Phúc Yên", "Tam Đảo", "Yên Lạc" } },
        { "Yên Bái", new List<string> { "Yên Bái", "Nghĩa Lộ", "Văn Chấn", "Mù Cang Chải" } }
    };

    private readonly List<(string Country, string Code)> Countries = new()
    {
        ("Vietnam", "+84"),
        ("United States", "+1"),
        ("United Kingdom", "+44"),
        ("Canada", "+1"),
        ("Australia", "+61"),
        // Add more countries and their codes here
    };

    // Handle text change and present suitable items for phone numbers
    private void AutoSuggestBox_TextChangedPhoneNumber(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var country in Countries)
            {
                var found = splitText.All((key) =>
                {
                    return country.Country.ToLower().Contains(key) || country.Code.Contains(key);
                });
                if (found)
                {
                    suitableItems.Add($"{country.Country} {country.Code}");
                }
            }
            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }
    }

    // Handle text change and present suitable items
    private void AutoSuggestBox_TextChangedProvince(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        // Since selecting an item will also change the text,
        // only listen to changes caused by user entering text.
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var cat in Provinces)
            {
                var found = splitText.All((key) =>
                {
                    return cat.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add(cat);
                }
            }
            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }
    }

    private void AutoSuggestBox_TextChangedDistrict(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var districtList in Districts.Values)
            {
                foreach (var district in districtList)
                {
                    var found = splitText.All((key) =>
                    {
                        return district.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(district);
                    }
                }
            }
            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }
    }

    // Handle user selecting an item for provinces
    private void AutoSuggestBox_SuggestionChosenProvince(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var suggestionOutput = (TextBlock)FindName("SuggestionOutput");
        if (suggestionOutput != null)
        {
            suggestionOutput.Text = args.SelectedItem.ToString();
        }
        else
        {
            // Handle the case where the TextBlock is not found
            Debug.WriteLine("SuggestionOutput TextBlock not found.");
        }
    }

    // Handle user selecting an item for districts
    private void AutoSuggestBox_SuggestionChosenDistrict(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var suggestionOutput = (TextBlock)FindName("SuggestionOutput");
        if (suggestionOutput != null)
        {
            suggestionOutput.Text = args.SelectedItem.ToString();
        }
        else
        {
            // Handle the case where the TextBlock is not found
            Debug.WriteLine("SuggestionOutput TextBlock not found.");
        }
    }

    // Handle user selecting an item for phone numbers
    private void AutoSuggestBox_SuggestionChosenPhoneNumber(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var selectedCountry = args.SelectedItem.ToString();
        sender.Text = selectedCountry;
    }

    private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
    {
        // Hiển thị ContentDialog để đổi mật khẩu
        var result = await ChangePasswordDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            // Lấy thông tin từ các PasswordBox
            var currentPassword = CurrentPasswordBox.Password;
            var newPassword = NewPasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;

            if (currentPassword != null)
            {
                // Kiểm tra mật khẩu hiện tại có đúng không
                // Ví dụ: Gọi API hoặc truy vấn cơ sở dữ liệu
            }
            else
            {
                // Hiển thị thông báo lỗi nếu mật khẩu không khớp
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Mật khẩu không chính xác.",
                    CloseButtonText = "Đóng"
                };
                await errorDialog.ShowAsync();
            }

            // Kiểm tra nếu mật khẩu mới và xác nhận mật khẩu khớp nhau
            if (newPassword == confirmPassword)
            {
                // Xử lý logic thay đổi mật khẩu
                // Ví dụ: Gọi API hoặc cập nhật cơ sở dữ liệu
            }
            else
            {
                // Hiển thị thông báo lỗi nếu mật khẩu không khớp
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Mật khẩu mới và mật khẩu xác nhận không khớp.",
                    CloseButtonText = "Đóng"
                };
                await errorDialog.ShowAsync();
            }
        }
    }

    // Xử lý sự kiện cho nút "Lưu" trong ContentDialog
    private void ChangePasswordDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        // Logic khi nhấn nút Lưu sẽ được xử lý tại đây nếu cần
    }

    private async void TwoLayerVerification(object sender, RoutedEventArgs e)
    {
        var result = await VerificationDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            // Lấy thông tin từ các PasswordBox
            var originalPhoneNumber = OriginalPhoneNumberTextBox.Text;
            var verifyPhoneNumber = VerifyPhoneNumberTextBox.Text;

            if (originalPhoneNumber != null)
            {
                // Kiểm tra mật khẩu hiện tại có đúng không
                // Ví dụ: Gọi API hoặc truy vấn cơ sở dữ liệu
            }
            else
            {
                // Hiển thị thông báo lỗi nếu mật khẩu không khớp
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Số điện thoại không chính xác.",
                    CloseButtonText = "Đóng"
                };
                await errorDialog.ShowAsync();
            }

            // Kiểm tra nếu mật khẩu mới và xác nhận mật khẩu khớp nhau
            if (verifyPhoneNumber != null)
            {
                // Xử lý logic thay đổi mật khẩu
                // Ví dụ: Gọi API hoặc cập nhật cơ sở dữ liệu
            }
            else
            {
                // Hiển thị thông báo lỗi nếu mật khẩu không khớp
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Số điện thoại không hợp lệ",
                    CloseButtonText = "Đóng"
                };
                await errorDialog.ShowAsync();
            }
        }
    }

    private void TwoLayerVerificationDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        // Logic khi nhấn nút Lưu sẽ được xử lý tại đây nếu cần
    }
}
