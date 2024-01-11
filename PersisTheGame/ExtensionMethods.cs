using System.Text.Json;

namespace PersisTheGame;

static class ExtensionMethods
{
    public static T? DeepClone<T>(this T obj)
    {
        if (obj == null)
            return default;

        string json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(json);
    }

}