using System.Text.Json.Serialization;

namespace MovieAPIs.ResponseModels
{
    public class RelatedFilmsResponce
    {
        [JsonPropertyName("items")]
        public RelatedFilmsResponceFilm[] Films { get; set; }
    }
}
