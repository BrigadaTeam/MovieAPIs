using Newtonsoft.Json;

namespace MovieAPIs.Common.Responses
{
    /// <summary>
    /// Request response model for multiple items.
    /// </summary>
    /// <typeparam name="T">The type of response model.</typeparam>
    public class ItemsResponse<T>
    {
        /// <summary>
        /// Base array of response elements.
        /// </summary>
        [JsonProperty("items")]
        public T[] Items { get; set; }

        /// <summary>
        /// Films array of response elements.
        /// </summary>
        [JsonProperty("films")]
        T[] Films
        {
            set
            {
                Items = value;
            }
        }
        
        /// <summary>
        /// Releases array of response elements.
        /// </summary>
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
