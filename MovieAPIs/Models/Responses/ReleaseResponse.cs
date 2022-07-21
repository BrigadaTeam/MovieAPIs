using Newtonsoft.Json;

namespace MovieAPIs.Models
{
    public class ReleaseResponse
    {
        public int Page { get; set; }
        public int Total { get; set; }

        [JsonProperty("releases")]
        public FilmSearch[] Films { get; set; }
    }
}
