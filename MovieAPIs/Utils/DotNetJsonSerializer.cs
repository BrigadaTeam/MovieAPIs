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
        T ISerializer.Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, options);
        }
    }
}
