using Application.Shared.Resources;
using SolutionName.Application.Enums;
using SolutionName.Application.Exceptions;


namespace SolutionName.Application.Utilities.Extensions;

public static class ThrowExceptionExtension
{
    public static void ThrowIfNull<T>(this T value, HttpResponseStatus exceptionEnum = HttpResponseStatus.Exception)
    {
        if (value is null)
            throw new ApiException(exceptionEnum);
    }

    public static void ThrowIfNull<T>(this T value, string exceptionMessage)
    {
        if (value == null)
            throw new ApiException(exceptionMessage);
    }


    public static void ThrowIfNotNull<T>(this T value, HttpResponseStatus exceptionType)
    {
        if (value is not null)
            throw new ApiException(exceptionType);
    }

    public static void ThrowIfNotNull<T>(T value, Exception? exception = null)
    {
        if (exception != null)
            throw exception;

        if (value is not null)
            throw new ApiException(HttpResponseStatus.ValidationError,
                string.Format(SharedResources.ObjectMustBeNull, typeof(T).Name));
    }

    public static void ThrowIfNotFound<T>(T value, Exception? exception = null)
    {
        if (exception is not null)
            throw exception;

        if (value is null)
            throw new ApiException(HttpResponseStatus.ValidationError,
                string.Format(SharedResources.ObjectNotFound, typeof(T).Name));
    }

    public static void ThrowIfLessThan<T>(this T value, T conditionValue, Exception? exception = null)
        where T : IComparable
    {
        if (exception is not null)
            throw exception;

        value.ThrowIfNull(HttpResponseStatus.PropertyIsNull);
        if (Comparer<T>.Default.Compare(value, conditionValue) < 0)
            throw new ApiException(HttpResponseStatus.ValidationError,
                string.Format(SharedResources.LessThanValidator, nameof(value), conditionValue));
    }

    public static void ThrowIfGreaterThan<T>(this T value, T conditionValue, Exception? exception = null)
        where T : IComparable
    {
        if (exception is not null)
            throw exception;

        value.ThrowIfNull(HttpResponseStatus.PropertyIsNull);
        if (Comparer<T>.Default.Compare(value, conditionValue) > 0)
            throw new ApiException(HttpResponseStatus.ValidationError,
                string.Format(SharedResources.GreaterThanValidator, nameof(value), conditionValue));
    }

    public static void ThrowIfEquals<T>(this T value, T conditionValue, Exception? exception = null)
        where T : IComparable
    {
        if (exception is not null)
            throw exception;

        value.ThrowIfNull(HttpResponseStatus.PropertyIsNull);
        if (Comparer<T>.Default.Compare(value, conditionValue) == 0)
            throw new ApiException(HttpResponseStatus.ValidationError,
                string.Format(SharedResources.EqualValidator, nameof(value), conditionValue));
    }

    public static void ThrowIfNotEquals<T>(this T value, T conditionValue, Exception? exception = null)
        where T : IComparable
    {
        if (exception is not null)
            throw exception;

        value.ThrowIfNull(HttpResponseStatus.PropertyIsNull);
        if (Comparer<T>.Default.Compare(value, conditionValue) != 0)
            throw new ApiException(HttpResponseStatus.ValidationError,
                string.Format(SharedResources.EqualValidator, nameof(value), conditionValue));
    }

    public static void ThrowIfTrue(bool value, HttpResponseStatus httpResponseStatusType)
    {
        if (value) throw new ApiException(httpResponseStatusType);
    }

    public static void ThrowIfTrue(bool value, string message)
    {
        if (value) throw new ApiException(HttpResponseStatus.ValidationError, message);
    }


    public static void ThrowIfFalse(bool value, HttpResponseStatus httpResponseStatusType)
    {
        if (value is false) throw new ApiException(httpResponseStatusType);
    }


    public static void ThrowIfFalse(bool value, string message)
    {
        if (value is false) throw new ApiException(HttpResponseStatus.ValidationError, message);
    }
}