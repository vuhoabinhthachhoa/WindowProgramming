using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employees;

namespace Sale_Project.Helpers;
public class EmployeeValidator
{
    private readonly IDialogService _dialogService;
    public EmployeeValidator(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    public bool Validate(Employee employee)
    {
        if (!IsValidNumericValue(employee.Id))
        {
            _dialogService.ShowErrorAsync("Error", "Employee ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            _dialogService.ShowErrorAsync("Error", "Employee name is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(employee.CitizenId))
        {
            _dialogService.ShowErrorAsync("Error", "Employee citizen ID is invalid");
            return false;
        }
        if (string.IsNullOrWhiteSpace(employee.JobTitle))
        {
            _dialogService.ShowErrorAsync("Error", "Employee job title is invalid");
            return false;
        }
        if (!IsValidNumericValue(employee.Salary))
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
