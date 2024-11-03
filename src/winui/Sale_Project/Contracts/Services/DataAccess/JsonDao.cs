using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using static Sale_Project.IDao;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sale_Project.Core.Helpers;
using Newtonsoft.Json;

namespace Sale_Project;
public class JsonDao : IDao
{
    public Tuple<List<Customer>, int> GetCustomers(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    )
    {
        var path = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
@"..\..\..\..\..\..\MockData\customers.json");

        var Customers = new List<Customer>();
        var json = System.IO.File.ReadAllText(path);
        Customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(json);


        // Search
        var query = from e in Customers
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

        return new Tuple<List<Customer>, int>(
            result.ToList(),
            query.Count()
        );
    }

    public bool DeleteCustomer(int id)
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\customers.json");

        var Customers = new List<Customer>();
        var json = File.ReadAllText(path);
        Customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(json);

        var item = Customers.Find(e => e.ID == id);
        Customers.Remove(item);

        var convertedJson = JsonConvert.SerializeObject(Customers, Formatting.Indented);
        File.WriteAllText(path, convertedJson);
        return true;
    }

    public bool AddCustomer(Customer info)
    {
        var path = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
@"..\..\..\..\..\..\MockData\customers.json");

        var Customers = new List<Customer>();
        var json = File.ReadAllText(path);
        Customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(json);

        Customers.Add(info);
        var convertedJson = JsonConvert.SerializeObject(Customers, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        return true;
    }

    public bool UpdateCustomer(Customer info)
    {
        // write update logic here
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\customers.json");
        var Customers = new List<Customer>();
        var json = File.ReadAllText(path);
        Customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(json);

        // update logic
        var item = Customers.Find(e => e.ID == info.ID);
        item.Name = info.Name;
        item.Address = info.Address;
        item.Email = info.Email;
        item.Phonenumber = info.Phonenumber;
        item.IsDeleted = info.IsDeleted;

        var convertedJson = JsonConvert.SerializeObject(Customers, Formatting.Indented);
        File.WriteAllText(path, convertedJson);



        return true;
    }
}