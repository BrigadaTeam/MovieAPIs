using Newtonsoft.Json;

namespace MovieAPIs.Models
{
    public class ViewerReviewsResponse
    {
        public int TotalPages { get; set; }
        public int TotalPositiveReviews { get; set; }
        public int TotalNegativeReviews { get; set; }
        public int TotalNeutralReviews { get; set; }
        [JsonProperty("items")]
        public Review[] Reviews { get; set; }
    }
}
