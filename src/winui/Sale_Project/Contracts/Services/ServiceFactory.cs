﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project;
using Sale_Project.Services;

namespace Sale_Project.Services;

public class ServiceFactory
{
    private static Dictionary<string, Type> _choices = new Dictionary<string, Type>();

    public static void Register(Type parent, Type child)
    {
        _choices.Add(parent.Name, child);
    }
    public static object GetChildOf(Type parent)
    {
        Type type = _choices[parent.Name];
        object instance = Activator.CreateInstance(type);
        return instance;
    }
}