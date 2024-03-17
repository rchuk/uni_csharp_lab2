namespace Lab2.Utils;

public enum ChineseZodiac
{
    Rat = 0,
    Ox,
    Tiger,
    Rabbit,
    Dragon,
    Snake,
    Horse,
    Goat,
    Monkey,
    Rooster,
    Dog,
    Pig
}

public static class ChineseZodiacExtension
{
    public static ChineseZodiac FromDate(DateTime dateTime)
    {
        int index = (dateTime.Year - 4) % 12;
        
        return (ChineseZodiac)index;
    }
}
