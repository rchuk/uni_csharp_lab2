using Lab2.Utils;

namespace Lab2.Models;

public class Person : BaseBindable
{
    private string _firstName;
    private string _lastName;
    private string _email;
    private DateTime _birthDate;

    private bool _calculatedFieldsReady;
    private int _age;
    private bool _isAdult;
    private bool _isBirthdayToday;
    private ChineseZodiac _chineseSign;
    private WesternZodiac _sunSign;

    public Person(string firstName, string lastName, string email, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        BirthDate = birthDate;
    }

    public Person(string firstName, string lastName, string email)
        : this(firstName, lastName, email, DateTime.Today)
    {
        
    }

    public Person(string firstName, string lastName, DateTime birthDate)
        : this(firstName, lastName, "", birthDate)
    {
        
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (UpdatePropertyValidated(ref _firstName, value.Trim(), ValidateFirstName, nameof(FirstName)))
                CalculatedFieldsReady = false;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (UpdatePropertyValidated(ref _lastName, value.Trim(), ValidateLastName, nameof(LastName)))
                CalculatedFieldsReady = false;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (UpdatePropertyValidated(ref _email, value.Trim(), ValidateEmail, nameof(Email)))
                CalculatedFieldsReady = false;
        }
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            if (UpdatePropertyValidated(ref _birthDate, value, ValidateBirthDate, nameof(BirthDate)))
                CalculatedFieldsReady = false;
        }
    }

    public int Age
    {
        get => _age;
        private set => UpdateProperty(ref _age, value, nameof(Age));
    }

    public bool IsAdult
    {
        get => _isAdult;
        private set => UpdateProperty(ref _isAdult, value, nameof(IsAdult));
    }

    public bool IsBirthdayToday
    {
        get => _isBirthdayToday;
        private set => UpdateProperty(ref _isBirthdayToday, value, nameof(IsBirthdayToday));
    }
    
    public ChineseZodiac ChineseSign
    {
        get => _chineseSign;
        private set => UpdateProperty(ref _chineseSign, value, nameof(ChineseSign));
    }

    public WesternZodiac SunSign
    {
        get => _sunSign;
        private set => UpdateProperty(ref _sunSign, value, nameof(SunSign));
    }

    public bool CalculatedFieldsReady
    {
        get => _calculatedFieldsReady;
        private set => UpdateProperty(ref _calculatedFieldsReady, value, nameof(CalculatedFieldsReady));
    }

    public void CalculateProperties()
    {
        Task.Delay(2500).Wait(); // Emulates calculation delay
        
        Age = CalculateAge(BirthDate);
        IsAdult = CalculateIsAdult(Age);
        IsBirthdayToday = CalculateIsBirthdayToday(BirthDate);
        ChineseSign = ChineseZodiacExtension.FromDate(BirthDate);
        SunSign = WesternZodiacExtension.FromDate(BirthDate);
        CalculatedFieldsReady = true;
    }

    private void ValidateFirstName()
    {
        ValidateNotEmpty(_firstName, nameof(FirstName), "First name");
    }

    private void ValidateLastName()
    {
        ValidateNotEmpty(_lastName, nameof(LastName), "Last name");
    }

    private void ValidateEmail()
    {
        ValidateNotEmpty(_email, nameof(Email), "Email");
    }
    
    private void ValidateBirthDate()
    {
        ClearPropertyErrors(nameof(BirthDate));
        var age = CalculateAge(BirthDate);
        if (age < 0)
            AddPropertyError(nameof(BirthDate), "User age can't be negative");
        if (age >= 135)
            AddPropertyError(nameof(BirthDate), "User age must be less than 135 years");
    }

    private void ValidateNotEmpty(string propertyValue, string propertyName, string propertyNamePretty)
    {
        ClearPropertyErrors(propertyName);
        if (propertyValue.Length == 0)
            AddPropertyError(propertyName, $"{propertyNamePretty} can't be empty");
    }
    
    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        if (birthDate.AddYears(age) > today)
            --age;

        return age;
    }

    private static bool CalculateIsAdult(int age)
    {
        return age >= 18;
    }

    private static bool CalculateIsBirthdayToday(DateTime birthDate)
    {
        var today = DateTime.Today;

        return birthDate.Month == today.Month && birthDate.Day == today.Day;
    }
}
