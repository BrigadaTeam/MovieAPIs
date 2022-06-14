using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class Genre
    {
        [JsonPropertyName("genre")]
        public string Name { get; set; }
    }
}
