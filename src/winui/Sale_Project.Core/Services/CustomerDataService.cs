using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;

namespace Sale_Project.Core.Services;
public class CustomerDataService : ICustomerDataService
{
    private List<Customer> _allCustomers;

    public CustomerDataService()
    {
    }

    private static IEnumerable<Customer> AllCustomers()
    {
        return new List<Customer>()
        {
            //add sample customer data 
            new Customer()
            {
                Id = "1",
                Name = "John Doe",
                Email = "johndoe@mail.com",
                Phonenumber = "1234567890",
                Address = "1234 Main St",
            },
            new Customer()
            {
                Id = "2",
                Name = "Jane Doe",
                Email = "janedoe@mail.com",
                Phonenumber = "0987654321",
                Address = "5678 Elm"
            },
            new Customer()
            {
                Id = "3",
                Name = "John Smith",
                Email = "johnsmith@mail.com",
                Phonenumber = "1234567890",
                Address = "1234 Main St",
            },
        };
    }

    public async Task<IEnumerable<Customer>> LoadDataAsync()
    {
        _allCustomers ??= new List<Customer>(AllCustomers());

        await Task.CompletedTask;
        return _allCustomers;
    }
}