using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Core.Models.Employees;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Models;

namespace Sale_Project.ViewModels;

public partial class EmployeeAddViewModel : ObservableRecipient, INavigationAware
{
    private readonly IEmployeeService _employeeService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IAuthService _authService;
    private readonly EmployeeCreationRequestValidator _employeeCreationRequestValidator;

    [ObservableProperty]
    private EmployeeCreationRequest _employeeCreationRequest;

    [ObservableProperty]
    private RegistrationRequest _registrationRequest;

    public Employee CreatedEmployee
    {
        get; set;
    }


    public EmployeeAddViewModel(IEmployeeService employeeService, INavigationService navigationService, IDialogService dialogService, IAuthService authService, EmployeeCreationRequestValidator employeeCreationRequestValidator)
    {
        _employeeService = employeeService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _authService = authService;
        _employeeCreationRequestValidator = employeeCreationRequestValidator;
    }

    public void OnNavigatedTo(object parameter)
    {
        EmployeeCreationRequest = new EmployeeCreationRequest();
        RegistrationRequest = new RegistrationRequest();
    }

    public void OnNavigatedFrom()
    {
        EmployeeCreationRequest = null;
        RegistrationRequest = null;
        CreatedEmployee = null;
    }

    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(EmployeeViewModel).FullName!);
    }

    public async Task AddEmployee()
    {
        if (CreatedEmployee != null)
        {
            await _dialogService.ShowErrorAsync("Error", "Employee has been already added!");
            return;
        }
        if (!_employeeCreationRequestValidator.Validate(EmployeeCreationRequest))
        {
            return;
        }
        Employee employee = await _employeeService.AddEmployee(EmployeeCreationRequest);
        if (employee == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Employee added successfully");
        CreatedEmployee = employee;
    }

    public async Task Register()
    {
        if(CreatedEmployee == null)
        {
           await _dialogService.ShowErrorAsync("Error", "Please add employee first!");
            return;
        }
        RegistrationRequest.EmployeeId = CreatedEmployee.Id;
        RegistrationRequest.Role = UserRole.USER;

        // because the server does not check this
        if(RegistrationRequest.Username == null)
        {
            await _dialogService.ShowErrorAsync("Error", "Username cannot be null!");
            return;
        }

        if(await _authService.Register(RegistrationRequest))
        {
            await _dialogService.ShowSuccessAsync("Success", "Account for the employee registered successfully");
        }
    }
}
