using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class FilmDistributionsResponse
    {

        [JsonPropertyName("items")]
        public FilmDistributionsResponseItems[] FilmDistributions { get; set; }

    }
}
