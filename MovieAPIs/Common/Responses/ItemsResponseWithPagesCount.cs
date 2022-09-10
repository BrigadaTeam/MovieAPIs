using Newtonsoft.Json;

namespace MovieAPIs.Common.Responses
{
    public class ItemsResponseWithPagesCount<T> : ItemsResponse<T>
    {
        [JsonProperty("pagesCount")]
        public int PagesCount { get; set; }

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
