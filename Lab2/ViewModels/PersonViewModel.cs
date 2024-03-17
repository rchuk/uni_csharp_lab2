using System.ComponentModel;
using System.Windows;
using Lab2.Models;
using Lab2.ViewModels.Commands;

namespace Lab2.ViewModels;

public class PersonViewModel : BaseBindable
{
    private Person _person;

    public PersonViewModel()
    {
        Person = new Person("", "Last Name", "smth@gmail.com");
        ProceedCommand = new RelayCommand( _ => Task.Run(() => {
                IsInputActive = false;
                Person.CalculateProperties();
                IsInputActive = true;
            }),
            _ => Person.IsValid
        );
    }

    public RelayCommand ProceedCommand { get; }

    private bool _isInputActive = true;
    public bool IsInputActive
    {
        get => _isInputActive;
        private set => UpdateProperty(ref _isInputActive, value, nameof(IsInputActive));
    }
    
    public Person Person
    {
        get => _person;
        set
        {
            if (UpdateProperty(ref _person, value, nameof(Person)))
            {
                Person.PropertyChanged += OnPersonPropertyChanged;
            }
        }
    }
    
    private void OnBirthdayUpdate() {
        if (Person.GetPropertyErrors(nameof(Person.BirthDate)) is var errors && errors != null)
        {
            MessageBox.Show(errors.ElementAt(0), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    protected override void PrePropertyUpdated(string propertyName)
    {
        if (propertyName == nameof(Models.Person))
        {
            Person.PropertyChanged -= OnPersonPropertyChanged;
        }
    }

    private void OnPersonPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        switch (args.PropertyName)
        {
            case nameof(Person.BirthDate):
                OnBirthdayUpdate();
                break;
            case nameof(Person.IsValid):
                ProceedCommand.RaiseCanExecuteChanged();
                break;
        }
    }
}
