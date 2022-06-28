using System.IO;
using System.Text.Json;

namespace MovieAPIs.Utils
{
    internal class DotNetJsonSerializer : ISerializer
    {
        JsonSerializerOptions options;
        internal DotNetJsonSerializer(JsonSerializerOptions options)
        {
            this.options = options;
        }
        T ISerializer.Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options)!;
        }
    }
}
