namespace SolutionName.Application.Utilities.Extensions;

public static class DatetimeExtensions
{
    public static bool IsExpired(this DateTime specificDate)
    {
        return specificDate < DateTime.Now;
    }

    public static string? CustomDateTimeFormat(this DateTime? dateTime)
    {
        return dateTime?.ToString("dd.MM.yyyy HH:mm:ss");
    }

    public static string CustomDateFormat(this DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyy");
    }
}