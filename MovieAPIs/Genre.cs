using System.Text.Json.Serialization;

namespace MovieAPIs
{
    public class Genre
    {
        [JsonPropertyName("genre")]
        public string Name { get; set; }
    }
}
