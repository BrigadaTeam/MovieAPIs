using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class FilmSearchByFilterResponse
    {
        public int TotalPages { get; set; }

        [JsonPropertyName("items")]
        public Film[] Filmss { get; set; }
    }
}
