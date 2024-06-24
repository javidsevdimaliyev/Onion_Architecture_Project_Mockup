using SolutionName.Application.Models.Shared;

namespace SolutionName.Application.Models.Responses;

public class PrintResult
{
    public bool HasChange { get; set; } = false;
    public byte[] Content { get; set; }
    public List<ChangeableFieldData> ChangeableFields { get; set; } = new();
}