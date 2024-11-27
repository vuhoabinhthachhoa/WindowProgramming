using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employee;

namespace Sale_Project.Helpers;
public class EmployeeCreationRequestValidator
{

    private readonly IDialogService _dialogService;
    public EmployeeCreationRequestValidator()
    {
        _dialogService = App.GetService<IDialogService>();
    }
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

    // Helper method to validate salary
    public bool IsValidNumericValue(double? value)
    {
        return value.HasValue &&
               !double.IsNaN(value.Value) &&
               !double.IsInfinity(value.Value) &&
               value.Value >= 0;
    }
}
