using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class Genre
    {
        public int Id { get; set; }

        [JsonPropertyName("genre")]
        public string Name { get; set; }
    }
}
