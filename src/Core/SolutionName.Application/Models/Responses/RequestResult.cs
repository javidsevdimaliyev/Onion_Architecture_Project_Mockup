namespace SolutionName.Application.Models.Responses;

public class RequestResult<T> : RequestResult
{
    public T Data { get; private set; }

    public static RequestResult<T> Succeed(T data)
    {
        return new RequestResult<T>
        {
            Exception = null,
            ExceptionMessage = "",
            IsSuccess = true,
            Data = data
        };
    }

    public new static RequestResult<T> Failure(string message)
    {
        return new RequestResult<T>
        {
            Exception = null,
            ExceptionMessage = message,
            IsSuccess = false
        };
    }

    public new static RequestResult<T> Failure(Exception exception)
    {
        return new RequestResult<T>
        {
            Exception = exception,
            ExceptionMessage = exception.Message,
            IsSuccess = false
        };
    }
}

public class RequestResult
{
    public Exception Exception { get; protected set; }
    public bool IsSuccess { get; protected set; }
    public string ExceptionMessage { get; protected set; }

    public static RequestResult Failure(Exception exception)
    {
        return new RequestResult
        {
            Exception = exception,
            ExceptionMessage = exception.Message,
            IsSuccess = false
        };
    }

    public static RequestResult Failure(string message)
    {
        return new RequestResult
        {
            Exception = null,
            ExceptionMessage = message,
            IsSuccess = false
        };
    }

    public static RequestResult Failure()
    {
        return new RequestResult
        {
            Exception = null,
            ExceptionMessage = "",
            IsSuccess = false
        };
    }

    public static RequestResult Succeed()
    {
        return new RequestResult
        {
            Exception = null,
            ExceptionMessage = "",
            IsSuccess = true
        };
    }
}