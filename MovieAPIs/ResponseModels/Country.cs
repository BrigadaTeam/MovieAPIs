using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class Country
    {
        [JsonPropertyName("country")]
        public string Name { get; set; }
    }
}
