using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Contracts.Services;

public interface IEmployeeDao
{
    public enum SortType
    {
        Ascending,
        Descending
    }
    Tuple<List<Employee>, int> GetEmployees(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    );

    bool DeleteEmployee(int id);
    (bool,string) AddEmployee(Employee info);
    (bool,string) UpdateEmployee(Employee info);
}