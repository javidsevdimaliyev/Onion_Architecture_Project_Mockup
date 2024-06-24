namespace SolutionName.Application.Shared.Utilities.Extensions;

public static class NumberExtentions
{
    public static int ToDivInt(this decimal value)
    {
        return Convert.ToInt32(value - value % 1);
    }

    public static int ToModInt(this decimal value)
    {
        return Convert.ToInt32(value % 1 * 100);
    }

    public static bool IsNumericType(this object o)
    {
        switch (Type.GetTypeCode(o.GetType()))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }

    //decimal
    public static decimal? SetNull(this decimal? value)
    {
        return value.Equals(0.00M) ? null : value;
    }

    public static decimal? SetDefaultValue(this decimal? value)
    {
        return value.Equals(null) ? 0.00M : value;
    }
}