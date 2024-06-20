using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace SolutionName.Application.Shared.Utilities.Extensions;

public static class CustomMapperExtension
{
    public static bool IsList(this object obj, IQueryable<long> officer)
    {
        if (obj == null) return false;
        return obj is IList &&
               obj.GetType().IsGenericType &&
               obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
    }

    //public static T Map<T>(this object mapObject, params string[] ignoreProperties)
    //    where T : class
    //{
    //    return mapObject.Map<T>(null, null, ignoreProperties);
    //}
    //public static T Map<T>(this object mapObject, List<MapMember> members = null, params string[] ignoreProperties)
    //    where T : class
    //{
    //    return mapObject.Map<T>(null, members, ignoreProperties);
    //}

    //public static T Map<T>(this object mapObject, T resultObject, params string[] ignoreProperties)
    //    where T : class
    //{
    //    return mapObject.Map<T>(resultObject, null, ignoreProperties);
    //}

    public static IEnumerable<T> Map<T>(this IEnumerable mapObject, IEnumerable<T> resultObject = null,
        List<MapMember> members = null, params string[] ignoreProperties)
        where T : class
    {
        if (mapObject == null) return null;

        var result = (List<T>)resultObject ?? new List<T>();
        foreach (var item in mapObject) result.Add(item.Map<T>(null, members, ignoreProperties));

        return result;
    }

    public static T Map<T>(this object mapObject, T resultObject = null, List<MapMember> members = null,
        params string[] ignoreProperties)
        where T : class
    {
        if (members == null)
            members = new List<MapMember>();


        if (mapObject == null)
            return null;

        var target = typeof(T);
        var instance = resultObject ?? Activator.CreateInstance(target, false);
        var targetPropertyList = target.GetProperties().ToList();
        foreach (var targetProperty in targetPropertyList.Where(x => x.GetSetMethod(true) != null))
        {
            if (ignoreProperties != null && ignoreProperties.Contains(targetProperty.Name))
                continue;
            PropertyInfo mapObjectProperty = null;

            if (members.Any(x => x.To.Equals(targetProperty.Name)))
            {
                var fromName = members.FirstOrDefault(k => k.To.Equals(targetProperty.Name)).From;
                mapObjectProperty = mapObject.GetType().GetProperties()
                    .FirstOrDefault(x => x.CanWrite && x.Name.Equals(fromName));
            }
            else
            {
                mapObjectProperty = mapObject.GetType().GetProperties()
                    .FirstOrDefault(x => x.CanWrite && x.Name.Equals(targetProperty.Name));
            }


            if (mapObjectProperty != null)
            {
                if (!mapObjectProperty.PropertyType.IsSimple()) continue;
                var mapObjectValue = mapObjectProperty?.GetValue(mapObject, null);
                if (mapObjectValue is string)
                    mapObjectValue = mapObjectValue.ToString().Trim();

                targetProperty.SetValue(instance, mapObjectValue, null);
            }
        }

        return (T)instance;
    }


    public static T MapProperty<T>(this object mapObject, string propertyName, T resultObject,
        string mapPropertyName = null) where T : class
    {
        var target = typeof(T);
        var instance = resultObject ?? Activator.CreateInstance(target, null);

        var resultProperty = target.GetProperty(propertyName);
        if (resultProperty != null)
        {
            var mapProperty = mapObject.GetType().GetProperty(mapPropertyName ?? propertyName);
            var mapValue = mapProperty?.GetValue(mapObject, null);

            if (resultProperty.PropertyType == mapProperty?.PropertyType)
            {
                resultProperty.SetValue(instance, mapValue, null);
            }
            else
            {
                if (!resultProperty.PropertyType.IsArray)
                {
                    var typeConverter = TypeDescriptor.GetConverter(resultProperty.PropertyType);
                    var value = typeConverter.ConvertFromString(mapValue?.ToString());
                    resultProperty.SetValue(instance, value, null);
                }
            }
        }

        return (T)instance;
    }

    #region MapFromNestedProperty

    public static IEnumerable<T> MapNested<T>(this IEnumerable mapObject, List<MapMember> members = null,
        params string[] ignoreProperties)
        where T : class
    {
        if (mapObject == null) return null;

        var result = new List<T>();
        foreach (var item in mapObject) result.Add(item.MapNested<T>(members, ignoreProperties));

        return result;
    }

    public static T MapNested<T>(this object mapObject, List<MapMember> members = null,
        params string[] ignoreProperties)
        where T : class
    {
        if (members == null)
            members = new List<MapMember>();

        if (mapObject == null)
            return null;

        var target = typeof(T);
        var instance = Activator.CreateInstance(target, false);
        var targetPropertyList = target.GetProperties().ToList();
        foreach (var targetProperty in targetPropertyList.Where(x => x.GetSetMethod(true) != null))
        {
            if (ignoreProperties != null && ignoreProperties.Contains(targetProperty.Name))
                continue;

            PropertyInfo mapObjectProperty = null;
            object mapObjectValue = null;
            if (members.Any(x => x.To.Equals(targetProperty.Name)))
            {
                var Fromname = members.FirstOrDefault(k => k.To.Equals(targetProperty.Name)).From;
                var test2 = mapObject.GetPropertyValue(Fromname.Split(".")[0]).GetPropertyValue(Fromname.Split(".")[1]);
                var i = 0;
                foreach (var item in Fromname.Split("."))
                {
                    if (i == 0)
                        mapObjectValue = mapObject.GetPropertyValue(item);

                    if (i > 0)
                        mapObjectValue = mapObjectValue.GetPropertyValue(item);

                    i++;
                }
            }
            else
            {
                mapObjectValue = mapObject.GetPropertyValue(targetProperty.Name);
            }


            if (mapObjectValue != null)
            {
                if (mapObjectValue is string)
                    mapObjectValue = mapObjectValue.ToString().Trim();

                targetProperty.SetValue(instance, mapObjectValue, null);
            }
        }

        return (T)instance;
    }

    #endregion
}

public class PropertyWork<TEntity> where TEntity : class
{
    public static PropertyInfo[] GetProperties()
    {
        return typeof(TEntity).GetProperties();
    }

    public static PropertyInfo GetProperty(string propertyName)
    {
        return !string.IsNullOrEmpty(propertyName) ? typeof(TEntity).GetProperty(propertyName) : null;
    }
}

public class PropertyWork
{
    public static PropertyInfo[] GetProperties(object obj)
    {
        return obj?.GetType().GetProperties();
    }

    public static PropertyInfo[] GetProperties(object obj, Type[] propertyTypes)
    {
        return obj?.GetType().GetProperties().Where(x => propertyTypes.Contains(x.PropertyType)).ToArray();
    }

    public static object GetValue(object obj, PropertyInfo property)
    {
        return property.GetValue(obj, null);
    }

    public static object GetValue(object obj, string propertyName)
    {
        if (!string.IsNullOrEmpty(propertyName))
            return obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null);

        return null;
    }
}

public static class TypeExtension
{
    public static PropertyInfo[] GetPropertiesWithoutAttribute<T>(this Type type) where T : Attribute
    {
        return type.GetProperties().Where(x => !Attribute.IsDefined(x, typeof(T))).ToArray();
    }

    public static PropertyInfo[] GetPropertiesWithoutAttribute<T>(this object obj) where T : Attribute
    {
        return obj.GetType().GetPropertiesWithoutAttribute<T>();
    }

    public static PropertyInfo GetProperty(this object obj, string propertyName)
    {
        if (!string.IsNullOrEmpty(propertyName))
            return obj?.GetType().GetProperty(propertyName);

        return null;
    }

    public static PropertyInfo GetPropertyWithoutAttribute<T>(this Type type, string propertyName) where T : Attribute
    {
        if (!string.IsNullOrEmpty(propertyName))
            return type.GetPropertiesWithoutAttribute<T>().FirstOrDefault(x => x.Name == propertyName);

        return null;
    }

    public static PropertyInfo GetPropertyWithoutAttribute<T>(this object obj, string propertyName) where T : Attribute
    {
        return obj.GetType().GetPropertyWithoutAttribute<T>(propertyName);
    }

    public static object GetPropertyValue(this object obj, string propertyName)
    {
        return obj.GetProperty(propertyName)?.GetValue(obj, null);
    }


    public static bool IsSimple(this Type type)
    {
        var typeInfo = type.GetTypeInfo();
        if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            // nullable type, check if the nested type is simple.
            return typeInfo.GetGenericArguments()[0].IsSimple();
        return typeInfo.IsPrimitive
               || typeInfo.IsEnum
               || type.Equals(typeof(string))
               || type.Equals(typeof(DateTime))
               || type.Equals(typeof(object))
               || type.Equals(typeof(decimal))
               || type.Equals(typeof(TimeSpan));
    }
}

public class MapMember
{
    public MapMember(string from, string to)
    {
        From = from;
        To = to;
    }

    public string From { get; set; }
    public string To { get; set; }
}