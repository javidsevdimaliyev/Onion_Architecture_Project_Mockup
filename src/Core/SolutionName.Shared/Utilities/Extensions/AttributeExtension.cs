using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Application.Shared.Utilities.Extensions;

public static class AttributeExtension
{
    public static string GetDomainName<T>(this T value)
    {
        var dnAttribute1 = typeof(T).GetCustomAttributes(true);
        var dnAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true);
        //if (dnAttribute != null)
        //{
        //    return dnAttribute.Name;
        //}
        return null;
    }
    public static string DescriptionAttr<T>(this T source)
    {
        var fi = source?.GetType().GetField(source.ToString());

        var attributes = (DescriptionAttribute[])fi?.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes?.Length > 0 ? attributes[0].Description : source?.ToString();
    }

    public static string GetDisplayName<T>(Expression<Func<T, object>> propertyExpression)
    {
        Expression expression;
        if (propertyExpression.Body.NodeType == ExpressionType.Convert)
            expression = ((UnaryExpression)propertyExpression.Body).Operand;
        else
            expression = propertyExpression.Body;

        if (expression.NodeType != ExpressionType.MemberAccess)
            throw new ArgumentException("Must be a property expression.", "propertyExpression");

        var me = (MemberExpression)expression;
        var member = me.Member;
        var att = member.GetCustomAttributes(typeof(DisplayAttribute), false).OfType<DisplayAttribute>()
            .FirstOrDefault();
        if (att != null)
            return att.Name;
        // No attribute found, just use the actual name.
        return member.Name;
    }
}