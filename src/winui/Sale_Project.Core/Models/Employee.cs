using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class Employee : INotifyPropertyChanged
{
    public int ID
    {
        get; set;
    } = 0;

    public string Name
    {
        get; set;
    } = "";

    public string Phonenumber
    {
        get; set;
    } = "";

    public string CitizenID
    {
        get; set;
    } = "";

    public string JobTitle
    {
        get; set;
    } = "";

    public int Salary
    {
        get; set;
    } = 0;

    public string Email
    {
        get; set;
    } = "";

    //public DateOnly DateOfBirth
    //{
    //    get; set;
    //} = DateOnly.MinValue;

    //private DateTime dateOfBirth;
    //public DateTime DateOfBirth
    //{
    //    get => dateOfBirth.Date;
    //    set
    //    {
    //        if (dateOfBirth.Date != value.Date)
    //        {
    //            dateOfBirth = value.Date;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfBirth)));
    //        }
    //    }
    //}

    public DateOnly DateOfBirth
    {
        get;
        set;
    } = DateOnly.MinValue;

    public string Address
    {
        get; set;
    } = "";

    public string Area
    {
        get; set;
    } = "";

    public string Ward
    {
        get; set;
    } = "";
    public string EmployeeStatus
    {
        get; set;
    } = "";

    public event PropertyChangedEventHandler PropertyChanged;
}