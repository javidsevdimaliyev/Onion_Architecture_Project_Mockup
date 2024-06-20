using SolutionName.Application.Models.Shared;
using SolutionName.Application.Utilities.Utility;

namespace SolutionName.Application.Models.Requests;


public class PagingRequest : EncryptionDto
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 15;
    public List<PagingRequestFilter> Filters { get; set; }
    public List<SortModel>? SortOptions { get; set; } = new();
}

public sealed class PagingRequestFilter : EncryptionDto
{
    public object Value { get; set; }
    public string FieldName { get; set; }
    public string EqualityType { get; set; }

    [JsonIgnore] public bool IsValidEqualityType => Enum.TryParse(EqualityType, out EqualityType type);
}

public class PageableRequest
{
    public virtual bool IsDescending { get; set; }
    public virtual bool IsDeleted { get; set; }
    public virtual string OrderByDescProperty { get; set; } = "Id";
    public int? Skip { get; set; }
    public int? Take { get; set; }

    public string GetOrderScript()
    {
        var orderbytype = " ASC";
        if (IsDescending)
            orderbytype = " DESC";

        return $"{OrderByDescProperty} {orderbytype}";
    }
}


public enum EqualityType
{
    Equal,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    StartsWith,
    EndsWith,
    Contains,
    DoesNotContain
}

public class EncryptionDto
{
    protected string? Encrypt(object? id)
    {
        if (string.IsNullOrEmpty(id.ToString()))
            return null;

        return TextEncryption.Encrypt(id!.ToString());
    }

    protected T? Decrypt<T>(string? id)
    {
        return TextEncryption.Decrypt<T>(id);
    }

    protected int[] Decrypt(string[] idsHash)
    {
        return TextEncryption.Decrypt(idsHash);
    }

    protected string[] Encrypt(int[] ids)
    {
        if (ids == null)
            return null;

        var idsHash = new string[ids.Length];

        for (var i = 0; i < ids.Length; i++) idsHash[i] = TextEncryption.Encrypt(ids[i].ToString());

        return idsHash;
    }
}

public class BasePagingDTO<T> : EncryptionDto
{
    public long Count { get; set; }

    public ICollection<T> Data { get; set; }
}