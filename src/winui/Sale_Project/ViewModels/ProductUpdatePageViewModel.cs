using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Services;

namespace Sale_Project;
public partial class ProductUpdatePageViewModel
{
    IProductDao _dao;
    public ProductUpdatePageViewModel()
    {
        _dao = ServiceFactory.GetChildOf(typeof(IProductDao)) as IProductDao;
    }
    public Product Info { get; set; } = new Product();
    public Stream FileStream
    {
        get; set;
    }
    public (bool, string) UpdateProduct()
    {
        return _dao.UpdateProduct(Info);
    }
}