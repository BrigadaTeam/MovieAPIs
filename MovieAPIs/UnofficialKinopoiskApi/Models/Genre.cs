using Newtonsoft.Json;

namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [JsonProperty("genre")]
        public string Name { get; set; }
    }
}
