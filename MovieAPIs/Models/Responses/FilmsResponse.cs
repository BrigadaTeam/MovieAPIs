using Newtonsoft.Json;

namespace MovieAPIs.Models
{
    public class FilmsResponse<T>
    {
        [JsonProperty("films")]
        public T[] Films { get; set; }
        [JsonProperty("items")]
        T[] Items
        {
            set
            {
                Films = value;
            }
        }
    }
}
