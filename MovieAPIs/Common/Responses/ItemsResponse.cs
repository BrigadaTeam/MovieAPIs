using Newtonsoft.Json;

namespace MovieAPIs.Common.Responses
{
    public class ItemsResponse<T>
    {
        [JsonProperty("items")]
        public T[] Items { get; set; }

        [JsonProperty("films")]
        T[] Films
        {
            set
            {
                Items = value;
            }
        }
        [JsonProperty("releases")]
        T[] Releases
        {
            set
            {
                Items = value;
            }
        }
    }
}
