using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class FilmFactsAndMistakesResponse
    {

        [JsonPropertyName("items")]
        public FilmFactsAndMistakesResponseItem[] FactsAndMistakesItems { get; set; }

    }
}
