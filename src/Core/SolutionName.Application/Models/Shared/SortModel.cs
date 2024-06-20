namespace SolutionName.Application.Models.Shared;

public class SortModel
{
    public SortModel()
    {
    }

    public SortModel(string column, bool isDesc) : this()
    {
        ColId = column;
        var orderByType = " ASC";
        if (isDesc)
            orderByType = " DESC";
        Sort = orderByType;
    }

    public string ColId { get; set; }
    public string Sort { get; set; }

    public string PairAsSqlExpression => $"{ColId} {Sort}";
}