using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class FilmFactsAndMistakesResponse
    {
        public int Total { get; set; }

        [JsonPropertyName("items")]
        public FilmFactsAndMistakesResponseItem[] FactsAndMistakes { get; set; }

    }
}
