using MovieAPIs.Common.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace MovieAPIs.UnofficialKinopoiskApi.Configuration
{
    /// <summary>
    /// Constant elements of apis methods.
    /// </summary>
    static internal class UnofficialKinopoiskConstants
    {
        public const string FilmsUrlV22 = "https://kinopoiskapiunofficial.tech/api/v2.2/films";

        public const string FiltersUrlV22 = "https://kinopoiskapiunofficial.tech/api/v2.2/films/filters";

        public const string PremieresUrlV22 = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres";

        public const string TopUrlV22 = "https://kinopoiskapiunofficial.tech/api/v2.2/films/top";

        public const string SearchByKeywordUrlV21 = "https://kinopoiskapiunofficial.tech/api/v2.1/films/search-by-keyword";

        public const string FilmsUrlV21 = "https://kinopoiskapiunofficial.tech/api/v2.1/films";

        public const string ReleasesUrlV21 = "https://kinopoiskapiunofficial.tech/api/v2.1/films/releases";

        public const string StaffUrlV1 = "https://kinopoiskapiunofficial.tech/api/v1/staff";

        public const string PersonsUrlV1 = "https://kinopoiskapiunofficial.tech/api/v1/persons";

        public const string SeasonsPathSegment = "seasons";

        public const string FactsPathSegment = "facts";

        public const string DistributionsPathSegment = "distributions";

        public const string BoxOfficePathSegment = "box_office";

        public const string AwardsPathSegment = "awards";

        public const string VideosPathSegment = "videos";

        public const string SimilarsPathSegment = "similars";

        public const string ImagesPathSegment = "images";

        public const string ReviewsPathSegment = "reviews";

        public const string SequelsAndPrequelsPathSegment = "sequels_and_prequels";

        public const int NumberFirstPage = 1;
    }
}
