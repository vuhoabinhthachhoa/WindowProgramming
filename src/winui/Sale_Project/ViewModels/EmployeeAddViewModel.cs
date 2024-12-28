using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Helpers;
using Sale_Project.Core.Models.Employees;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Models;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for adding a new employee.
/// </summary>
public partial class EmployeeAddViewModel : ObservableRecipient, INavigationAware
{
    private readonly IEmployeeService _employeeService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IAuthService _authService;
    private readonly EmployeeCreationRequestValidator _employeeCreationRequestValidator;
    private GlobalKeyboardHook _globalKeyboardHook;



    /// <summary>
    /// Gets or sets the employee creation request.
    /// </summary>
    [ObservableProperty]
    private EmployeeCreationRequest _employeeCreationRequest;

    /// <summary>
    /// Gets or sets the registration request.
    /// </summary>
    [ObservableProperty]
    private RegistrationRequest _registrationRequest;

    /// <summary>
    /// Gets or sets the created employee.
    /// </summary>
    public Employee CreatedEmployee
    {
        get; set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeAddViewModel"/> class.
    /// </summary>
    /// <param name="employeeService">The employee service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="dialogService">The dialog service.</param>
    /// <param name="authService">The authentication service.</param>
    /// <param name="employeeCreationRequestValidator">The employee creation request validator.</param>
    public EmployeeAddViewModel(IEmployeeService employeeService, INavigationService navigationService, IDialogService dialogService, IAuthService authService, EmployeeCreationRequestValidator employeeCreationRequestValidator)
    {
        _employeeService = employeeService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _authService = authService;
        _employeeCreationRequestValidator = employeeCreationRequestValidator;
    }

    /// <summary>
    /// Called when navigated to the view.
    /// </summary>
    /// <param name="parameter">The navigation parameter.</param>
    public void OnNavigatedTo(object parameter)
    {
        EmployeeCreationRequest = new EmployeeCreationRequest();
        RegistrationRequest = new RegistrationRequest();

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
        EmployeeCreationRequest = null;
        RegistrationRequest = null;
        CreatedEmployee = null;

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
            await AddEmployee();
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
    /// Adds a new employee.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Registers a new account for the created employee.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Register()
    {
        if (CreatedEmployee == null)
        {
            await _dialogService.ShowErrorAsync("Error", "Please add employee first!");
            return;
        }
        RegistrationRequest.EmployeeId = CreatedEmployee.Id;
        RegistrationRequest.Role = UserRole.USER;

        // because the server does not check this
        if (RegistrationRequest.Username == null)
        {
            await _dialogService.ShowErrorAsync("Error", "Username cannot be null!");
            return;
        }

        if (await _authService.Register(RegistrationRequest))
        {
            await _dialogService.ShowSuccessAsync("Success", "Account for the employee registered successfully");
        }
    }
}
