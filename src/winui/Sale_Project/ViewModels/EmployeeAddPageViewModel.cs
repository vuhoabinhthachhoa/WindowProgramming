using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Services;

namespace Sale_Project;
public partial class EmployeeAddPageViewModel
{
    public EmployeeAddPageViewModel()
    {
        _dao = ServiceFactory.GetChildOf(typeof(IEmployeeDao)) as IEmployeeDao;
    }

    public Employee Info { get; set; } = new Employee();
    IEmployeeDao _dao;

    public (bool, string) AddEmployee()
    {
        return _dao.AddEmployee(Info);
        //string message = result ? "Employee added successfully." : "Failed to add employee.";
        //return (result, message);
    }
}
