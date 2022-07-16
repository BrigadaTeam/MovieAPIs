using Newtonsoft.Json;

namespace MovieAPIs.Utils
{
    internal class NewtonsoftJsonSerializer : ISerializer
    {
        T ISerializer.Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}
