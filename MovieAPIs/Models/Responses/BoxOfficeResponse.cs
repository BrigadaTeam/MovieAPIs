using Newtonsoft.Json;

namespace MovieAPIs.Models.Responses
{
    public class BoxOfficeResponse
    {
        [JsonProperty("items")]
        public MonetizationInfo[] MonetizationInfo { get; set; }
    }
}
