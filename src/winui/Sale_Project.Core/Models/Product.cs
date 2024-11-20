using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class Product : INotifyPropertyChanged 
{
    public int ID
    {
        get; set;
    } = 0;

    public string Name
    {
        get; set;
    } = "";
     
    public string CategoryID
    {
        get; set;
    } = "";

    public int ImportPrice
    {
        get; set;
    } = 0;

    public int SellingPrice
    {
        get; set;
    } = 0;

    public string BranchID
    {
        get; set;
    } = "";

    public int InventoryQuantity
    {
        get; set;
    } = 0;

    public string Images
    {
        get; set;
    } = "";

    public string BusinessStatus
    {
        get; set;
    } = "Đang kinh doanh";

    public string Size
    {
        get; set;
    } = "";

    public double DiscountPercent
    {
        get; set;
    } = 0;

    public event PropertyChangedEventHandler PropertyChanged;
}
