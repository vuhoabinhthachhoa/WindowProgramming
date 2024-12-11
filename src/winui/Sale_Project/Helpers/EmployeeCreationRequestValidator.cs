using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employees;

namespace Sale_Project.Helpers;
/// <summary>
/// Validates the employee creation request.
/// </summary>
public class EmployeeCreationRequestValidator
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeCreationRequestValidator"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service to show error messages.</param>
    public EmployeeCreationRequestValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Validates the specified employee creation request.
    /// </summary>
    /// <param name="employeeCreationRequest">The employee creation request to validate.</param>
    /// <returns><c>true</c> if the request is valid; otherwise, <c>false</c>.</returns>
    public bool Validate(EmployeeCreationRequest employeeCreationRequest)
    {
        if (string.IsNullOrWhiteSpace(employeeCreationRequest.Name))
        {
            _dialogService.ShowErrorAsync("Error", "Employee name is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(employeeCreationRequest.CitizenId))
        {
            _dialogService.ShowErrorAsync("Error", "Employee citizen ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(employeeCreationRequest.JobTitle))
        {
            _dialogService.ShowErrorAsync("Error", "Employee job title is invalid");
            return false;
        }
        if (!IsValidNumericValue(employeeCreationRequest.Salary))
        {
            _dialogService.ShowErrorAsync("Error", "Employee salary is invalid");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Determines whether the specified value is a valid numeric value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
    public bool IsValidNumericValue(double? value)
    {
        return value.HasValue &&
               !double.IsNaN(value.Value) &&
               !double.IsInfinity(value.Value) &&
               value.Value >= 0;
    }
}
