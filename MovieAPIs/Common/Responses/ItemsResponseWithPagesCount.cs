using Newtonsoft.Json;

namespace MovieAPIs.Common.Responses
{
    /// <summary>
    /// Request response model for multiple items with pagination.
    /// </summary>
    /// <typeparam name="T">The type of response model.</typeparam>
    public class ItemsResponseWithPagesCount<T> : ItemsResponse<T>
    {
        /// <summary>
        /// Number of pages in the request.
        /// </summary>
        [JsonProperty("pagesCount")]
        public int PagesCount { get; set; }
        
        /// <summary>
        /// Number of pages in the request.
        /// </summary>
        [JsonProperty("totalPages")]
        int TotalPages
        {
            set
            {
                PagesCount = value;
            }
        }
    }
}
