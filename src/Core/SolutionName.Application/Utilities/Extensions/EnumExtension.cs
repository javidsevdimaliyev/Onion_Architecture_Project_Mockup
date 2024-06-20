using SolutionName.Application.Utilities.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace SolutionName.Application.Utilities.Extensions;

public static class EnumExtension
{
    public static string GetDescription(this Enum enumValue)
    {
        var fi = enumValue.GetType().GetField(enumValue.ToString());

        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        return enumValue.ToString();
    }

    public static string GetDescription<T>(this T e, string? property = null) where T : IConvertible
    {
        string description = null;
        var additionalInfo = property is not null ? " - " + property : string.Empty;
        if (e is Enum)
        {
            var type = e.GetType();
            var values = Enum.GetValues(type);

            foreach (int val in values)
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val));
                    var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descriptionAttributes.Length > 0)
                        description = ((DescriptionAttribute)descriptionAttributes[0]).Description + additionalInfo;

                    break;
                }
        }

        return description;
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        var displayName = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();

        if (string.IsNullOrEmpty(displayName)) displayName = enumValue.ToString();

        return displayName;
    }

    public static string GetDisplayName<T>(this T value) where T : struct
    {
        var type = value.GetType();
        var fieldInfo = type.GetField(value.ToString());
        if (fieldInfo == null) return Enum.GetName(type, value);

        if (!(fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) is DisplayAttribute[] descriptionAttributes)
            || descriptionAttributes.Length == 0) return value.ToString();

        if (descriptionAttributes[0].ResourceType != null)
            return EnumHelper<T>.LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

        return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Name : value.ToString();
    }

    public static T ToEnum<T>(this string value) where T : struct
    {
        T tmp;
        if (!Enum.TryParse(value, true, out tmp)) tmp = new T();
        return tmp;
    }
}