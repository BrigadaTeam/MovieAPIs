using MovieAPIs.Common.Responses;
using Newtonsoft.Json;

namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class ViewerReviewsResponse<T> : ItemsResponseWithPagesCount<T>
    {
        public int TotalPositiveReviews { get; set; }
        public int TotalNegativeReviews { get; set; }
        public int TotalNeutralReviews { get; set; }
    }
}
