﻿using SolutionName.Application.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace SolutionName.Application.Shared.Utilities.Helpers;

public static class EnumHelper<T>
{
    public static int ToInt(Enum enumValue)
    {
        return Convert.ToInt32(enumValue);
    }

    public static T ToEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static List<EnumStructureModel> GetDisplayStructure()
    {
        return (from T item in Enum.GetValues(typeof(T))
            select new EnumStructureModel
            {
                Id = Convert.ToInt32(item),
                Value = Enum.GetName(typeof(T), item),
                DisplayName = GetDisplayValue(item)
            }).ToList();
    }

    public static string GetDisplayNameById(T id)
    {
        return GetDisplayStructure().FirstOrDefault(x => x.Id == Convert.ToInt32(id))
            ?.DisplayName;
    }

    public static IList<T> GetValues(Enum value)
    {
        var enumValues = new List<T>();

        foreach (var fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
        return enumValues;
    }

    public static T Parse(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static IList<string> GetNames(Enum value)
    {
        return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
    }

    public static IList<string> GetDisplayValues(Enum value)
    {
        return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
    }

    public static string LookupResource(Type resourceManagerProvider, string resourceKey)
    {
        foreach (var staticProperty in resourceManagerProvider.GetProperties(
                     BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            if (staticProperty.PropertyType == typeof(ResourceManager))
            {
                var resourceManager =
                    (ResourceManager)staticProperty.GetValue(null, null);
                return resourceManager.GetString(resourceKey);
            }

        return resourceKey; // Fallback with the key name
    }

    public static string GetDisplayValue(T value)
    {
        try
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Name : value.ToString();
        }
        catch (Exception e)
        {
            return "";
        }
    }
}