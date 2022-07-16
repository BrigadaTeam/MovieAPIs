using System.IO;
using System.Text.Json;

namespace MovieAPIs.Utils
{
    internal interface ISerializer
    {
        internal T Deserialize<T>(string json);
    }

    internal class DotNetJsonSerializer : ISerializer
    {
        readonly JsonSerializerOptions options;
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
