namespace Lab2.Utils;

public enum WesternZodiac
{
    Capricorn = 0,
    Aquarius,
    Pisces,
    Aries,
    Taurus,
    Gemini,
    Cancer,
    Leo,
    Virgo,
    Libra,
    Scorpio,
    Sagittarius
}

public static class WesternZodiacExtension
{
    public static WesternZodiac FromDate(DateTime dateTime)
    {
        int[] pivotDays = [21, 20, 22, 21, 22, 22, 24, 24, 24, 24, 23, 23];

        int monthIndex = dateTime.Month - 1;
        int offset = dateTime.Day < pivotDays[monthIndex] ? 0 : 1;
        int index = (monthIndex + offset) % 12;

        return (WesternZodiac)index;
    }
}
