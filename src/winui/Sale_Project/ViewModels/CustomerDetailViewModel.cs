using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sale_Project.Core.Models;

namespace Sale_Project.ViewModels;

public partial class CustomerDetailViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<Voucher> vouchers = new();

    [ObservableProperty]
    private ObservableCollection<Voucher> currentVouchers = new();

    public bool IsPreviousButtonEnabled => CurrentPage >= 0;

    public bool IsNextButtonEnabled => CurrentPage < TotalPages - 1;

    private int currentPage;

    public int CurrentPage
    {
        get => currentPage;
        set
        {
            if (value >= 0 && value < TotalPages && SetProperty(ref currentPage, value))
            {
                UpdateCurrentVouchers();
                OnPropertyChanged(nameof(CurrentVouchers));
                OnPropertyChanged(nameof(CurrentPageDisplay));
                OnPropertyChanged(nameof(IsPreviousButtonEnabled));
                OnPropertyChanged(nameof(IsNextButtonEnabled));
            }
        }
    }

    public ICommand PreviousPageCommand => new RelayCommand(PreviousPage);
    public ICommand NextPageCommand => new RelayCommand(NextPage);

    private void PreviousPage()
    {
        if (IsPreviousButtonEnabled)
        {
            CurrentPage--;
        }
    }

    private void NextPage()
    {
        if (IsNextButtonEnabled)
        {
            CurrentPage++;
        }
    }

    public int CurrentPageDisplay => CurrentPage + 1;

    private const int itemsPerPage = 5;

    public int TotalPages => Vouchers.Count == 0 ? 0 : (int)Math.Ceiling((double)Vouchers.Count / itemsPerPage);

    public CustomerDetailViewModel()
    {
        Vouchers = new ObservableCollection<Voucher>
        {
            new() { Code = "()2024", ExpiredDate = DateTime.Now.AddMonths(1), Discount = 10.0, MaxDiscount = 50.0, Status = "Active" },
            new() { Code = "SPRINGSALE", ExpiredDate = DateTime.Now.AddMonths(2), Discount = 15.0, MaxDiscount = 100.0, Status = "Active" },
            new() { Code = "SUMMERSALE", ExpiredDate = DateTime.Now.AddMonths(3), Discount = 20.0, MaxDiscount = 75.0, Status = "Expired" },
            new() { Code = "FALL2024", ExpiredDate = DateTime.Now.AddMonths(4), Discount = 5.0, MaxDiscount = 20.0, Status = "Active" },
            new() { Code = "BLACKFRIDAY", ExpiredDate = DateTime.Now.AddDays(10), Discount = 30.0, MaxDiscount = 150.0, Status = "Active" },
            new() { Code = "WINTERSALE", ExpiredDate = DateTime.Now.AddMonths(5), Discount = 25.0, MaxDiscount = 125.0, Status = "Active" },
            new() { Code = "NEWYEAR2025", ExpiredDate = DateTime.Now.AddMonths(6), Discount = 35.0, MaxDiscount = 200.0, Status = "Expired" },
            new() { Code = "CHRISTMAS", ExpiredDate = DateTime.Now.AddDays(30), Discount = 40.0, MaxDiscount = 250.0, Status = "Active" },
            new() { Code = "EASTER2025", ExpiredDate = DateTime.Now.AddMonths(7), Discount = 50.0, MaxDiscount = 300.0, Status = "Active" },
            new() { Code = "SUMMERSUN", ExpiredDate = DateTime.Now.AddMonths(8), Discount = 20.0, MaxDiscount = 100.0, Status = "Active" },
            new() { Code = "AUTUMNLEAVES", ExpiredDate = DateTime.Now.AddMonths(9), Discount = 10.0, MaxDiscount = 50.0, Status = "Expired" },
            new() { Code = "WINTERWONDER", ExpiredDate = DateTime.Now.AddMonths(10), Discount = 25.0, MaxDiscount = 150.0, Status = "Active" },
            new() { Code = "SPRING2025", ExpiredDate = DateTime.Now.AddMonths(11), Discount = 15.0, MaxDiscount = 80.0, Status = "Active" },
            new() { Code = "SUMMERFUN", ExpiredDate = DateTime.Now.AddMonths(12), Discount = 20.0, MaxDiscount = 100.0, Status = "Expired" },
            new() { Code = "FALLFRENZY", ExpiredDate = DateTime.Now.AddMonths(13), Discount = 5.0, MaxDiscount = 20.0, Status = "Active" },
            new() { Code = "BLACKFRIDAY2025", ExpiredDate = DateTime.Now.AddDays(15), Discount = 30.0, MaxDiscount = 150.0, Status = "Active" },
            new() { Code = "HOLIDAY2025", ExpiredDate = DateTime.Now.AddMonths(14), Discount = 25.0, MaxDiscount = 125.0, Status = "Active" },
            new() { Code = "NEWYEAR2026", ExpiredDate = DateTime.Now.AddMonths(15), Discount = 35.0, MaxDiscount = 200.0, Status = "Expired" },
            new() { Code = "CHRISTMAS2025", ExpiredDate = DateTime.Now.AddDays(20), Discount = 40.0, MaxDiscount = 250.0, Status = "Active" },
            new()
            {
                Code = "EASTER2026",
                ExpiredDate = DateTime.Now.AddMonths(16),
                Discount = 50.0,
                MaxDiscount = 300.0,
                Status = "Active"
            }
        };

        CurrentPage = 0;
        UpdateCurrentVouchers();
    }

    private void UpdateCurrentVouchers()
    {
        var items = Vouchers.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();

        CurrentVouchers.Clear();
        foreach (var item in items)
        {
            CurrentVouchers.Add(item);
        }
    }
}