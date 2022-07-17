using Newtonsoft.Json;

namespace MovieAPIs.Utils
{
    internal interface ISerializer
    {
        internal T Deserialize<T>(string json);
    }

    internal class NewtonsoftJsonSerializer : ISerializer
    {
        T ISerializer.Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}
