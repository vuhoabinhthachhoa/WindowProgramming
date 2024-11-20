using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Services;

namespace Sale_Project;
public partial class ProductAddPageViewModel
{
    public ProductAddPageViewModel()
    {
        _dao = ServiceFactory.GetChildOf(typeof(IProductDao)) as IProductDao;
    }

    public Product Info { get; set; } = new Product();
    IProductDao _dao;

    public (bool, string) AddProduct()
    {
        return _dao.AddProduct(Info);
        //string message = result ? "Product added successfully." : "Failed to add Product.";
        //return (result, message);
    }
}
