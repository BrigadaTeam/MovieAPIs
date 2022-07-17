using Newtonsoft.Json;

namespace MovieAPIs.Models.Responses
{
    public class FactsAndMistakesResponse
    {
        [JsonProperty("items")]
        public FilmPeculiarity[] FilmPeculiarities { get; set; }
    }
}
