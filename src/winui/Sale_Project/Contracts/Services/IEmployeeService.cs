using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employee;

namespace Sale_Project.Contracts.Services;
public interface IEmployeeService
{
    // Add a new employee to the system
    Task<Employee> AddEmployee(EmployeeCreationRequest employee);

    // Mark an employee as unemployed (inactive)
    Task<bool> UnemployedEmployee(long employeeId);

    // Update an existing employee's details
    Task<Employee> UpdateEmployee(Employee employee);

    // Optionally, add a method to retrieve employees
    //Task<Employee> GetEmployeeById(long employeeId);

    // Optionally, add a method to list all employees
    Task<IEnumerable<Employee>> GetAllEmployees();

    Task<PageData<Employee>> SearchEmployees(int page, int size, string sortField, SortType sortType, EmployeeSearchRequest employeeSearchRequest);

    Task<Employee> GetEmployeeById(long employeeId);

}
