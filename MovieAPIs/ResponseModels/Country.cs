using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class Country
    {
        public int Id { get; set; }

        [JsonPropertyName("country")]
        public string Name { get; set; }
    }
}
