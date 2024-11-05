using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using static Sale_Project.Contracts.Services.IEmployeeDao;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sale_Project.Core.Helpers;
using Newtonsoft.Json;
using Sale_Project.Contracts.Services;
using System.Runtime.CompilerServices;

namespace Sale_Project.Services.Dao.JsonDao;
public class EmployeeJsonDao : IEmployeeDao
{

    public (bool, string) IsValidInfo(Employee info)
    {
        if (info == null) return (false, "Employee info is null");

        if (string.IsNullOrWhiteSpace(info.Name) || info.Name.Any(char.IsDigit)) return (false, "Invalid Name");
        if (string.IsNullOrWhiteSpace(info.Phonenumber) || !info.Phonenumber.All(char.IsDigit)) return (false, "Invalid Phonenumber");
        if (string.IsNullOrWhiteSpace(info.CitizenID)) return (false, "CitizenID is required");
        if (string.IsNullOrWhiteSpace(info.JobTitle)) return (false, "JobTitle is required");
        if (info.Salary <= 0) return (false, "Salary must be greater than 0");
        if (string.IsNullOrWhiteSpace(info.Email) || !IsValidEmail(info.Email)) return (false, "Invalid Email");
        if (info.DateOfBirth==DateTime.MinValue) return (false, "DateOfBirth is required");
        if (string.IsNullOrWhiteSpace(info.Address)) return (false, "Address is required");
        //if (string.IsNullOrWhiteSpace(info.Area)) return (false, "Area is required");
        //if (string.IsNullOrWhiteSpace(info.Ward)) return (false, "Ward is required");

        return (true, string.Empty);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public string GetJsonFilePath(string fileName)
    {
        var fullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var index = fullPath.IndexOf(@"Sale_Project");

        if (index != -1)
        {
            var basePath = fullPath.Substring(0, index);

            return Path.Combine(basePath, @"Sale_Project\MockData\", fileName);
        }
        else
        {
            throw new InvalidOperationException("Invalid path");
        }
    }


    public Tuple<List<Employee>, int> GetEmployees(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    )
    {
        //        var path = Path.Combine(
        //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        //@"..\..\..\..\..\..\MockData\employees.json");

        var path = GetJsonFilePath("employees.json");

        var Employees = new List<Employee>();
        var json = File.ReadAllText(path);
        Employees = System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(json);

        // Search
        var query = from e in Employees
                    where e.Name.ToLower().Contains(keyword.ToLower())
                    select e;

        //// Filter
        //int min = 3;
        //int max = 15;
        //query = query.Where(item => item.ID > min && item.ID < max);

        // Sort
        foreach (var option in sortOptions)
        {
            if (option.Key == "ID")
            {
                if (option.Value == SortType.Ascending)
                {
                    query = query.OrderBy(e => e.ID);
                }
                else
                {
                    query = query.OrderByDescending(e => e.ID);
                }
            }
        }

        var result = query
            .Skip((page - 1) * rowsPerPage)
            .Take(rowsPerPage);

        return new Tuple<List<Employee>, int>(
            result.ToList(),
            query.Count()
        );
    }

    public bool DeleteEmployee(int id)
    {
        var path = GetJsonFilePath("employees.json");

        var Employees = new List<Employee>();
        var json = File.ReadAllText(path);
        Employees = System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(json);

        var item = Employees.Find(e => e.ID == id);
        Employees.Remove(item);

        var convertedJson = JsonConvert.SerializeObject(Employees, Formatting.Indented);
        File.WriteAllText(path, convertedJson);
        return true;
    }

    public (bool,string) AddEmployee(Employee info)
    {
        var (isValid, message) = IsValidInfo(info);
        if (!isValid) return (false, message);

        var path = GetJsonFilePath("employees.json");

        var Employees = new List<Employee>();
        var json = File.ReadAllText(path);
        Employees = System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(json);

        Employees.Add(info);
        var convertedJson = JsonConvert.SerializeObject(Employees, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        return (true, string.Empty);
    }

    public (bool,string) UpdateEmployee(Employee info)
    {
        var (isValid, message) = IsValidInfo(info);
        if (!isValid) return (false, message);

        var path = GetJsonFilePath("employees.json");
        var Employees = new List<Employee>();
        var json = File.ReadAllText(path);
        Employees = System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(json);

        var item = Employees.Find(e => e.ID == info.ID);
        item.Name = info.Name;
        item.Phonenumber = info.Phonenumber;
        item.CitizenID = info.CitizenID;
        item.JobTitle = info.JobTitle;
        item.Salary = info.Salary;
        item.Email = info.Email;
        item.DateOfBirth = info.DateOfBirth;
        item.Address = info.Address;
        item.Area = info.Area;
        item.Ward = info.Ward;
        item.EmployeeStatus = info.EmployeeStatus;

        var convertedJson = JsonConvert.SerializeObject(Employees, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        return (true, string.Empty);
    }
}