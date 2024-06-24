using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolutionName.Application.Utilities.Converters;

public class DateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.TryGetDateTime(out var value)) value = DateTime.Parse(reader.GetString()!);
        return value;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date?.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"));
    }
}