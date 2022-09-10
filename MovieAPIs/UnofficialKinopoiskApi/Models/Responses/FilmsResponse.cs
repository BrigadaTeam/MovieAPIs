using Newtonsoft.Json;

namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class FilmsResponse<T>
    {
        [JsonProperty("films")]
        
        // TODO come up with a more generalized name because in this field there are series and other items
        public T[] Films { get; set; }
        [JsonProperty("items")]
        T[] Items
        {
            set
            {
                Films = value;
            }
        }
        [JsonProperty("releases")]
        T[] Releases
        {
            set
            {
                Films = value;
            }
        }
    }
}
