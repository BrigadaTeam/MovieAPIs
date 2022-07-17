using Newtonsoft.Json;
using MovieAPIs.Models.Serials;

namespace MovieAPIs.Models.Responses
{
    public class SerialResponse
    {
        [JsonProperty("items")]
        public Season[] Seasons { get; set; }

    }
}
