using System.Collections;

namespace SolutionName.Application.Models.Responses;

public class TablePageableResult
{
    public int TotalCount { get; set; }
    public IEnumerable List { get; set; }
}

public class TablePageableResult<T>
{
    public long TotalCount { get; set; } = 0;
    public IEnumerable<T> List { get; set; } = new List<T>();
}