using System.Collections;
using System.ComponentModel;

namespace Lab2.Models;

public abstract class BaseBindable : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _propertyErrors = new ();
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public bool IsValid => !HasErrors;
    public bool HasErrors => _propertyErrors.Count > 0;
    
    public IEnumerable GetErrors(string? propertyName)
    {
        return GetPropertyErrors(propertyName);
    }

    public IEnumerable<string>? GetPropertyErrors(string? propertyName)
    {
        if (propertyName == null)
            return _propertyErrors.Values.SelectMany(x => x);
        
        return _propertyErrors.GetValueOrDefault(propertyName);
    }

    protected bool UpdateProperty<T>(ref T property, T value, string propertyName, params string[] dependantPropertyNames)
    {
        return UpdatePropertyValidated(ref property, value, null, propertyName, dependantPropertyNames);
    }
    
    protected bool UpdatePropertyValidated<T>(ref T property, T value, Action? validate, string propertyName, params string[] dependantPropertyNames)
    {
        if (EqualityComparer<T>.Default.Equals(property, value))
            return false;
        
        if (property != null)
            PrePropertyUpdated(propertyName);
        property = value;
        
        bool wasValid = IsValid;
        validate?.Invoke();
        if (wasValid != IsValid)
            OnPropertyChanged(nameof(IsValid));
        
        OnPropertyChanged(propertyName);
        foreach (var name in dependantPropertyNames)
            OnPropertyChanged(name);

        return true;
    }
    
    protected void AddPropertyError(string propertyName, string error)
    {
        if (!_propertyErrors.TryGetValue(propertyName, out var errorList))
        {
            errorList = new List<string>();
            _propertyErrors.Add(propertyName, errorList);
        }
        
        errorList.Add(error);
        OnErrorsChanged(propertyName);
    }

    protected void ClearPropertyErrors(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
            OnErrorsChanged(propertyName);
    }

    protected virtual void PrePropertyUpdated(string propertyName) {}

    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
