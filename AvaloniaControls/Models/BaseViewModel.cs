using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvaloniaControls.Models;

public class BaseViewModel : INotifyPropertyChanged
{
    private bool _hasBeenModified;
    public bool HasBeenModified
    {
        get => _hasBeenModified;
        set
        {
            SetField(ref _hasBeenModified, value);
            OnPropertyChanged(nameof(_hasBeenModified));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        if (propertyName != nameof(HasBeenModified))
        {
            HasBeenModified = true;    
        }
        
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}