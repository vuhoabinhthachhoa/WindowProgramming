using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class Customer : INotifyPropertyChanged
{
    public int ID
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    public string Email
    {
        get; set;
    } = "";

    public string Phonenumber
    {
        get; set;
    }

    public string Address
    {
        get; set;
    } = "";

    public bool IsDeleted { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
}