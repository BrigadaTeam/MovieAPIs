using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class FilmDistributionsResponse
    {
        public int Total { get; set; }

        [JsonPropertyName("items")]
        public FilmDistributionsResponseItems[] Distributions { get; set; }

    }
}
