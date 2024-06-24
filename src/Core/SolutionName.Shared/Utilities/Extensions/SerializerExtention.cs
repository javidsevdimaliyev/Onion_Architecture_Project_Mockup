using System.Text.Json;

namespace SolutionName.Application.Shared.Utilities.Extensions;

public static class SerializerExtention
{
    public static string GetSerialized(this object actionResult)
    {
        return JsonSerializer.Serialize(actionResult, new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            WriteIndented = false
        });
    }

    public static T GetDeserialized<T>(this string actionResult)
    {
        if (string.IsNullOrEmpty(actionResult)) actionResult = "{}";
        return JsonSerializer.Deserialize<T>(actionResult, new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        });
    }
}