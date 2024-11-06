using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Services;

namespace Sale_Project;
public partial class EmployeeUpdatePageViewModel
{
    IEmployeeDao _dao;
    public EmployeeUpdatePageViewModel()
    {
        _dao = ServiceFactory.GetChildOf(typeof(IEmployeeDao)) as IEmployeeDao;
    }
    public Employee Info { get; set; } = new Employee();

    public (bool, string) UpdateEmployee()
    {
        return _dao.UpdateEmployee(Info);
    }
}