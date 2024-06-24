using System.Globalization;
using System.Text;
using System.Text.Json;
using SolutionName.Application.Shared.Utilities.Utility;

namespace SolutionName.Application.Shared.Utilities.Extensions;

public static class StringExtension
{
    /// <summary>
    ///     EqualsIgnoreCase
    /// </summary>
    /// <param name="s1">string1</param>
    /// <param name="s2">string2</param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string? s1, string? s2)
    {
        return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     ContainsIgnoreCase
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public static bool ContainsIgnoreCase(this string? s1, string? s2)
    {
        if (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2))
            return s1.Trim().Contains(s2.Trim(), StringComparison.OrdinalIgnoreCase);

        return false;
    }

    public static bool IsValidJson(this string text)
    {
        text = text.Trim();
        if ((text.StartsWith("{") && text.EndsWith("}")) || // object
            (text.StartsWith("[") && text.EndsWith("]"))) // array
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(text))
                {
                    // Successfully parsed the JSON
                }
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static string Replace(this string text, Dictionary<string, string> elements)
    {
        return elements.Aggregate(text, (current, element) => current.Replace(element.Key, element.Value));
    }

    public static string SubstringInterval(this string text, string startKey, string lastKey)
    {
        var startIndex = text.IndexOf(startKey, StringComparison.Ordinal) + startKey.Length;
        var endIndex = text.LastIndexOf(lastKey, StringComparison.Ordinal);
        var length = endIndex - startIndex;
        return text.Substring(startIndex, length);
    }

    public static CultureInfo CurrentCulture => new("az");
    public static string ToUpperCulture(this string text)
    {
        if (text == null) return text;

        return text.ToUpper(CurrentCulture);
    }

    public static int ToInt(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return 0;

        return Convert.ToInt32(text.Replace(".", ","));
    }

    public static decimal ToDecimal(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return 0;

        //return Convert.ToDecimal(text.Replace(".", ","));
        return decimal.Parse(text, CultureInfo.InvariantCulture);
    }

    public static string ConvertStringToDate(this string text)
    {
        var dateArray = new string[3];
        if (!string.IsNullOrEmpty(text))
            dateArray = text.Split('-');
        else
            return null;

        var dateString = "";
        if (dateArray.Count() > 2)
            dateString = $"{dateArray[2].Substring(0, 2)}.{dateArray[1]}.{dateArray[0]}";
        else
            dateString = $"{dateArray[0]}";

        return dateString;
    }

    public static string FromLeft(this string str, int maxLength)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        return str.Substring(0, Math.Min(str.Length, maxLength));
    }

    public static string GetHash(this string str)
    {
        return $"{str.GetHashCode():x8}";
    }

    public static string Suffix(this string name)
    {
        var suffix = "";
        // var tt = name[name.Length - 1].ToString();
        if (name != null && !string.IsNullOrEmpty(name.Trim()) && name.Trim() != "")
            switch (name.Trim()[name.Trim().Length - 1].ToString())
            {
                case "A":
                case "I":
                    suffix = "NIN";
                    break;
                case "E":
                case "Ə":
                case "İ":
                    suffix = "NİN";
                    break;
                case "O":
                case "U":
                    suffix = "NUN";
                    break;
                case "Ö":
                case "Ü":
                    suffix = "NÜN";
                    break;
                default:
                    switch (name.Trim()[name.Trim().Length - 2].ToString())
                    {
                        case "A":
                        case "I":
                            suffix = "IN";
                            break;
                        case "E":
                        case "Ə":
                        case "İ":
                            suffix = "İN";
                            break;
                        case "O":
                        case "U":
                            suffix = "UN";
                            break;
                        case "Ö":
                        case "Ü":
                            suffix = "ÜN";
                            break;
                    }

                    break;
            }

        return suffix;
    }

    public static string Suffix2(this string name)
    {
        var suffix = "";
        if (name != null && !string.IsNullOrEmpty(name.Trim()) && name.Trim() != "")
            switch (name.Trim()[name.Trim().Length - 1].ToString())
            {
                case "A":
                case "O":
                    suffix = "YA";
                    break;
                case "E":
                case "Ə":
                case "İ":
                    suffix = "NƏ";
                    break;
                case "I":
                case "U":
                    suffix = "NA";
                    break;
                case "Ö":
                case "Ü":
                    suffix = "YƏ";
                    break;
                default:
                    switch (name.Trim()[name.Trim().Length - 2].ToString())
                    {
                        case "A":
                        case "I":
                            suffix = "A";
                            break;
                        case "E":
                        case "Ə":
                        case "İ":
                            suffix = "Ə";
                            break;
                        case "O":
                        case "U":
                            suffix = "A";
                            break;
                        case "Ö":
                        case "Ü":
                            suffix = "Ə";
                            break;
                    }

                    break;
            }

        return suffix;
    }

    public static string EndYear(this string year)
    {
        var yearEnd = "-ci";
        if (year != null && !string.IsNullOrEmpty(year.Trim()) && year.Trim() != "")
            switch (year[year.Length - 1].ToString())
            {
                case "3":
                    yearEnd = "-cü";
                    break;
                case "4":
                    yearEnd = "-cü";
                    break;
                case "6":
                    yearEnd = "-cı";
                    break;
                case "9":
                    yearEnd = "-cu";
                    break;
                case "0":
                    switch (year[year.Length - 2].ToString())
                    {
                        case "1":
                            yearEnd = "-cu";
                            break;
                        case "3":
                            yearEnd = "-cu";
                            break;
                        case "4":
                            yearEnd = "-cı";
                            break;
                        case "6":
                            yearEnd = "-cı";
                            break;
                        case "9":
                            yearEnd = "-cı";
                            break;
                    }

                    break;
                default:
                    yearEnd = "-ci";
                    break;
            }

        return yearEnd;
    }

    public static string DateWithMonthName(this DateTime date)
    {
        return
            $"{date.Day} {date.ToString("MMMM", CultureInfo.CreateSpecificCulture("az")).ToLower()} {date.Year}{date.Year.ToString().EndYear()}";
    }


    public static string DecodeFromBase64(this string content)
    {
        var data = Convert.FromBase64String(content);
        return Encoding.UTF8.GetString(data);
    }

    /// <summary>
    ///     Parses a string and returns the corresponding decimal value.
    ///     The input string may contain numeric characters and either '.' or ',' as the decimal separator.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>The decimal value of the input string.</returns>
    public static decimal ParseAmount(this string input)
    {
        decimal value = default;
        // Only allow digits, '.' and ',' in the input string
        input = new string(input.Where(c => char.IsDigit(c) || c is '.' or ',').ToArray());
        if (decimal.TryParse(input.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture,
                out value))
            return value;

        if (decimal.TryParse(input, out value)) return value;

        // The string was not in a valid decimal format
        input = input.Replace(',', '.');
        var index = input.LastIndexOf('.');
        if (index == -1) return value;

        // Replace the last '.' with a ','
        input = input.Remove(index, 1).Insert(index, ",");
        return decimal.TryParse(input, out value) ? value : value;
    }
}