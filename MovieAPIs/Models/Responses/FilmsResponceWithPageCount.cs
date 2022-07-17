using Newtonsoft.Json;

namespace MovieAPIs.Models
{
    public class FilmsResponceWithPageCount<T> : FilmsResponse<T>
    {
        [JsonProperty("pagesCount")]
        public int PageCount { get; set; }

        [JsonProperty("totalPages")]
        int TotalPages
        {
            set
            {
                PageCount = value;
            }
        }
    }
}
