﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Sale_Project.Helpers;
public sealed class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
    where T : INotifyPropertyChanged
{
    public ObservableCollection()
    {
        CollectionChanged += FullObservableCollectionCollectionChanged;
    }

    public ObservableCollection(IEnumerable<T> pItems) : this()
    {
        foreach (var item in pItems)
        {
            Add(item);
        }
    }

    private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (Object item in e.NewItems)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
            }
        }
        if (e.OldItems != null)
        {
            foreach (Object item in e.OldItems)
            {
                ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
            }
        }
    }

    private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
        OnCollectionChanged(args);
    }
}
