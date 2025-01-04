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


/// <summary>
/// ViewModel for updating an employee.
/// </summary>
public partial class EmployeeUpdateViewModel : ObservableObject, INavigationAware
{
    private readonly IEmployeeService _employeeService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly EmployeeValidator _employeeValidator;
    private GlobalKeyboardHook _globalKeyboardHook;


    [ObservableProperty]
    private Employee _currentEmployee;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeUpdateViewModel"/> class.
    /// </summary>
    /// <param name="employeeService">The employee service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="dialogService">The dialog service.</param>
    /// <param name="employeeValidator">The employee validator.</param>
    public EmployeeUpdateViewModel(IEmployeeService employeeService, INavigationService navigationService, IDialogService dialogService, EmployeeValidator employeeValidator)
    {
        _employeeService = employeeService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _employeeValidator = employeeValidator;
    }

    /// <summary>
    /// Called when navigated to the view.
    /// </summary>
    /// <param name="parameter">The navigation parameter.</param>
    public async void OnNavigatedTo(object parameter)
    {
        CurrentEmployee = await _employeeService.GetEmployeeById((long)parameter);
        // Initialize the hook
        _globalKeyboardHook = new GlobalKeyboardHook();
        _globalKeyboardHook.SetHook();

        // Hook up the event
        _globalKeyboardHook.KeyPressed += OnGlobalKeyPressed;
    }

    /// <summary>
    /// Called when navigated from the view.
    /// </summary>
    public void OnNavigatedFrom()
    {
        CurrentEmployee = null;
        // Unhook and clean up
        if (_globalKeyboardHook != null)
        {
            _globalKeyboardHook.KeyPressed -= OnGlobalKeyPressed;
            _globalKeyboardHook.Unhook();
            _globalKeyboardHook = null;
        }
    }

    private async void OnGlobalKeyPressed(object sender, (int KeyCode, bool IsCtrlPressed) keyInfo)
    {
        // Detect Ctrl + S (KeyCode: 0x53 for 'S')
        if (keyInfo.KeyCode == 0x53 && keyInfo.IsCtrlPressed)
        {
            await UpdateEmployee();
        }
    }

    /// <summary>
    /// Navigates back to the previous view.
    /// </summary>
    public void GoBack()
    {
        _navigationService.NavigateTo(typeof(EmployeeViewModel).FullName!);
    }

    /// <summary>
    /// Updates the current employee.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateEmployee()
    {
        if (!_employeeValidator.Validate(CurrentEmployee))
        {
            return;
        }
        Employee employee = await _employeeService.UpdateEmployee(CurrentEmployee);
        if (employee == null)
        {
            return;
        }
        await _dialogService.ShowSuccessAsync("Success", "Employee updated successfully");
        CurrentEmployee = employee;
    }

    /// <summary>
    /// Marks the current employee as unemployed.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

