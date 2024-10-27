using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;

namespace Sale_Project.Core.Services;
public class EmployeeDataService : IEmployeeDataService
{
    private List<Employee> _allEmployees;

    public EmployeeDataService()
    {
    }

    private static IEnumerable<Employee> AllEmployees()
    {
        return new List<Employee>()
        {
            //add sample employee data
            new Employee()
            {
                Id = "1",
                Name = "Nguyen Van A",
                Phonenumber = "0123456789",
                Citizen_id = "123456789",
                Job_title = "Developer",
                Salary = 1000,
                Email = "nguyenvana@mail.com",
                Date_of_birth = "01/01/1990",
                Address = "123 Nguyen",
                Area = "Quan 1",
                Ward = "Phuong 1"
            },
            new Employee()
            {
                Id = "2",
                Name = "Nguyen Van B",
                Phonenumber = "0123456789",
                Citizen_id = "123456789",
                Job_title = "Developer",
                Salary = 1000,
                Email = "nguyenvanb@mail.com",
                Date_of_birth = "01/01/1990",
                Address = "123 Nguyen",
                Area = "Quan 1",
                Ward = "Phuong 1"
                },

            new Employee()
            {
                Id = "3",
                Name = "Nguyen Van C",
                Phonenumber = "0123456789",
                Citizen_id = "123456789",
                Job_title = "Developer",
                Salary = 1000,
                Email = "nguyenvanc@mail.com",
                Date_of_birth = "01/01/1990",
                Address = "123 Nguyen",
                Area = "Quan 1",
                Ward = "Phuong 1"
            }

        };
    }

    public async Task<IEnumerable<Employee>> LoadDataAsync()
    {
        _allEmployees ??= new List<Employee>(AllEmployees());

        await Task.CompletedTask;
        return _allEmployees;
    }
}