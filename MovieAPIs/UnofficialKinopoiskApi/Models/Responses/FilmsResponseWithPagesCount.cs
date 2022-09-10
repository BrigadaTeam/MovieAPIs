using Newtonsoft.Json;

namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class FilmsResponseWithPagesCount<T> : FilmsResponse<T>
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
