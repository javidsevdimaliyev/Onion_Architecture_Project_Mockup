using SolutionName.Application.Models.Shared;

namespace SolutionName.Application.Models.Requests;

public class PrintRequest
{
    public int PaddingTop { get; set; } = 0;
    public string FontFamily { get; set; } = "Tahoma";
    public int FontSize { get; set; } = 8;

    public decimal LineSpacing { get; set; }

    // public int PrintingId { get; set; }
    public PageSide PageSide { get; set; } = PageSide.FrontSide;
    public PageOrientation PageOrientation { get; set; } = PageOrientation.Portrait;

    /// <summary>
    ///     1-A4, 2 -A5
    /// </summary>
    // public int PageSize { get; set; } = 1;

    public PrintingFormat PrintingFormat { get; set; } = PrintingFormat.Pdf;

    public double MarginTop { get; set; }
    public double MarginBottom { get; set; }
    public double MarginLeft { get; set; }
    public double MarginRight { get; set; }
}

public class CanPrintSection
{
    public bool Images { get; set; } = true;
    public bool Persons { get; set; } = true;
    public bool Content { get; set; } = true;
    public bool RelatedDocuments { get; set; } = true;
}

public enum PageSide
{
    FrontSide = 1,
    BackSide = 2
}

public enum PageOrientation
{
    Portrait = 1,
    Landscape = 2
}

public enum PrintingFormat
{
    Pdf = 1,
    Html
}

public static class ChangeableFieldDataExtention
{
    public static List<ChangeableFieldData> OrderFields(this List<ChangeableFieldData> fields)
    {
        for (var i = 1; i <= fields.Count; i++)
        {
            var field = fields[i - 1];
            field.Key = $"Change{i}";
            field.Label = $"Bənd {i}";
            field.Id = i;
        }

        return fields;
    }
}