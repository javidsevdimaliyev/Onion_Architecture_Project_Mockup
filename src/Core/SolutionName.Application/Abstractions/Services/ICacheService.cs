namespace SolutionName.Application.Abstractions.Services;

public interface ICacheService
{
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> createFunc, TimeSpan? expiration = default);
}