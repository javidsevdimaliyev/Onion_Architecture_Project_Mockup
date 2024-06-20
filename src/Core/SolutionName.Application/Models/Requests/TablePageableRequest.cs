namespace SolutionName.Application.Models.Requests;

public class TablePageableRequest
{
    public List<string> Fields { get; set; } = new();
    public string TableName { get; set; } = "";
    public bool IsCountQuery { get; set; } = false;
    public bool Paging { get; set; } = true;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 0;


    public List<SortingInfo> Sort { get; set; } = new();
    public List<FilterInfo> Filter { get; set; } = new();
    public bool IsReport { get; set; }

    public string GetPagingScript()
    {
        if (Paging)
            return $"OFFSET {Skip} ROWS FETCH NEXT {Take} ROWS ONLY";

        return "";
    }

    public string GetOrderScript()
    {
        var script = "order by Id";
        if (Sort.Count > 0)
        {
            script = "order by ";
            script += string.Join(",", Sort.Select(x => x.ToString()).ToList());
        }

        return script;
    }
}

public class SortingInfo
{
    public string Selector { get; set; }
    public bool Desc { get; set; } = false;

    public override string ToString()
    {
        return $"{Selector} {(Desc ? "desc" : "")}";
    }
}

public class FilterInfo
{
    public string Field { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }
    public FieldType Type { get; set; }

    public override string ToString()
    {
        var filterString = $"{Field} ";
        string after = "", before = "";
        switch (Operator.Trim())
        {
            case "contains":
                {
                    after = "%";
                    before = "%";
                    filterString += "like ";
                }
                break;
            case "notcontains":
                {
                    after = "%";
                    before = "%";
                    filterString += "not like ";
                }
                break;
            case "startswith":
                {
                    after = "";
                    before = "%";
                    filterString += "like ";
                }
                break;
            case "endswith":
                {
                    after = "%";
                    before = "";
                    filterString += "like ";
                }
                break;
            case "in":
                {
                    after = "(";
                    before = ")";
                    filterString += "in ";
                }
                break;
            default:
                {
                    filterString += $"{Operator.Trim()} ";
                    after = "";
                    before = "";
                }
                break;
        }

        switch (Type)
        {
            case FieldType.Float:
            case FieldType.Integer:
                filterString += $"{Value}";
                break;
            case FieldType.TimeSpan:
            case FieldType.Date:
                filterString += $"'{Value}'";
                break;
            case FieldType.Guid:
            case FieldType.String:
                filterString += $"'{after}{Value}{before}'";
                break;
        }


        return filterString;
    }
}

public enum FieldType
{
    Float = 1,
    Integer,
    TimeSpan,
    Date,
    Guid,
    String
}

public enum QueryType
{
    Table = 1,
    StoredProcedur
}