using System.Reflection;

namespace SolutionName.Application.Utilities.Helpers
{

    public static class PropertyHelper<TEntity> where TEntity : class
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

    public static class PropertyHelper
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

}
