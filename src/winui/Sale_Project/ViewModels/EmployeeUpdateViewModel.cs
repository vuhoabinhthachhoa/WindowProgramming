using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Contracts.Services;
using Sale_Project.Services;
using Sale_Project.ViewModels;
using Sale_Project.Helpers;
using Sale_Project.Core.Models.Employees;


public partial class EmployeeUpdateViewModel : ObservableObject, INavigationAware
{
    private readonly IEmployeeService _employeeService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly EmployeeValidator _employeeValidator;

    [ObservableProperty]
    private Employee _currentEmployee;

    public EmployeeUpdateViewModel(IEmployeeService employeeService, INavigationService navigationService, IDialogService dialogService, EmployeeValidator employeeValidator)
    {
        _employeeService = employeeService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _employeeValidator = employeeValidator; 
    }

    public async void OnNavigatedTo(object parameter)
    {
       CurrentEmployee = await _employeeService.GetEmployeeById((long)parameter);
    }

    public void OnNavigatedFrom()
    {
        CurrentEmployee = null;
    }

    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(EmployeeViewModel).FullName!);
    }

    public async Task UpdateEmployee()
    {
        if (!_employeeValidator.Validate(CurrentEmployee))
        {
            return;
        }
        Employee employee = await _employeeService.UpdateEmployee(CurrentEmployee);
        if(employee == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Employee updated successfully");
        CurrentEmployee = employee;
    }

    public async Task MarkEmployeeUnemployed()
    {
        bool confirm = await _dialogService.ShowConfirmAsync("Confirm", "Are you sure you want to mark this employee as unemployed?");
        if (!confirm)
        {
            return;
        }

        bool result = await _employeeService.UnemployedEmployee(CurrentEmployee.Id);
        if (!result)
        {
            await _dialogService.ShowErrorAsync("Error", "Failed to mark employee as unemployed.");
            return;
        }

        CurrentEmployee.EmploymentStatus = false;
        await _dialogService.ShowSuccessAsync("Success", "Employee marked as unemployed successfully.");
    }
}
