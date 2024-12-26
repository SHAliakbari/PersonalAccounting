

using System.Globalization;

namespace System.Globalization;

public static class CalendarHelper
{
    public static string ConvertToPersianCalendar(this DateOnly dateTime)
    {
        PersianCalendar pc = new PersianCalendar();
        return $"{pc.GetYear(dateTime.ToDateTime(TimeOnly.MinValue))}-{pc.GetMonth(dateTime.ToDateTime(TimeOnly.MinValue))}-{pc.GetDayOfMonth(dateTime.ToDateTime(TimeOnly.MinValue))}";
    }
}
