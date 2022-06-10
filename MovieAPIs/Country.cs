using System.Text.Json.Serialization;

namespace MovieAPIs
{
    public class Country
    {
        [JsonPropertyName("country")]
        public string Name { get; set; }
    }
}
