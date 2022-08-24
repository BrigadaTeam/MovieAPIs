using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MovieAPIs.Configuration;
using MovieAPIs.Models;
using MovieAPIs.Utils;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient : MovieApiClientBase
    {
        readonly IHttpClient httpClient;
        readonly ISerializer serializer;
        readonly UnofficialKinopoiskConstants constants;
        readonly IManyRequestsHelper manyRequestsHelper;

        public UnofficialKinopoiskApiClient(string apiKey) : this(new InternalHttpClient(apiKey)) { }

        internal UnofficialKinopoiskApiClient(IHttpClient httpClient)
        {
            serializer = new NewtonsoftJsonSerializer();
            this.httpClient = httpClient;
            using (var reader = new StreamReader(Path.Combine("Configuration", "configuration.json")))
            {
                string json = reader.ReadToEnd();
                constants = serializer.Deserialize<UnofficialKinopoiskConstants>(json);
            }
            manyRequestsHelper = new ManyRequestsHelper(httpClient, serializer);
        }

        public async Task<Film> GetFilmByIdAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}";
            return await GetResponseDataAsync<Film>(path);
        }

        public async IAsyncEnumerable<FilmSearch> GetTopFilmsFromAllPagesAsync(Tops topType = Tops.TOP_250_BEST_FILMS)
        {
            var firstFilmsResponse = await GetTopFilmsAsync(topType);
            int pagesCount = firstFilmsResponse.PagesCount;
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString()            
            };
            string path = $"{constants.TopUrlV22}";
            var filmsResponses = manyRequestsHelper.GetDataFromAllPages<FilmSearch>(queryParams, pagesCount, 5, path);
            await foreach (var filmsResponse in filmsResponses)
            {
                yield return filmsResponse;
            }
        }
        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetFilmsByKeywordAsync(string keyword, int page = 1)
        {
            string path = $"{constants.SearchByKeywordUrlV21}";
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<FilmSearch>>(path, queryParams);
        }

        public async Task<GenresAndCountriesResponse> GetGenresAndCountriesAsync()
        {
            string path = $"{constants.FiltersUrlV22}";
            return await GetResponseDataAsync<GenresAndCountriesResponse>(path);
        }

        public async Task<FilmsResponseWithPagesCount<Film>> GetFilmsByFiltersAsync(int countryId = (int)Filter.ALL,
            int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
            MovieOrder order = MovieOrder.RATING, MovieType type = MovieType.ALL, int ratingFrom = 0, int ratingTo = 10,
            int yearFrom = 1000, int yearTo = 3000, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["countries"] = countryId == (int)Filter.ALL ? "" : countryId.ToString(),
                ["genres"] = genreId == (int)Filter.ALL ? "" : genreId.ToString(),
                ["order"] = order.ToString(),
                ["type"] = type.ToString(),
                ["ratingFrom"] = ratingFrom.ToString(),
                ["ratingTo"] = ratingTo.ToString(),
                ["yearFrom"] = yearFrom.ToString(),
                ["yearTo"] = yearTo.ToString(),
                ["imdbId"] = imdbId,
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            string path = $"{constants.FiltersUrlV22}";
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<Film>>(path, queryParams);
        }

        public async Task<FilmsResponse<RelatedFilm>> GetRelatedFilmsAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SimilarsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<RelatedFilm>>(path);
        }

        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetTopFilmsAsync(
            Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            string path = $"{constants.TopUrlV22}";
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<FilmSearch>>(path, queryParams);
        }

        public async Task<FilmsResponse<FactsAndMistakes>> GetFilmFactsAndMistakesAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.FactsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<FactsAndMistakes>>(path);
        }

        public async Task<FilmsResponse<FilmDistributionsResponseItems>> GetFilmDistributionsAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.DistributionsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<FilmDistributionsResponseItems>>(path);
        }

        public async Task<FilmsResponseWithPagesCount<FilmRelease>> GetDigitalReleasesAsync(int year, Months month,
            int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
                ["page"] = page.ToString()
            };
            string path = $"{constants.ReleasesUrlV21}";
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<FilmRelease>>(path, queryParams);
        }

        public async Task<FilmSearch[]> GetSequelsAndPrequelsByIdAsync(int id)
        {
            string path = $"{constants.FilmsUrlV21}/{id}/{constants.SequelsAndPrequelsPathSegment}";
            return await GetResponseDataAsync<FilmSearch[]>(path);
        }

        public async Task<ViewerReviewsResponse> GetViewerReviewsByIdAsync(int id, int page = 1,
            ReviewOrder order = ReviewOrder.DATE_DESC)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["order"] = order.ToString(),
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ReviewsPathSegment}";
            return await GetResponseDataAsync<ViewerReviewsResponse>(path, queryParams);
        }

        public async Task<FilmsResponse<MonetizationInfo>> GetBoxOfficeByIdAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.BoxOfficePathSegment}";
            return await GetResponseDataAsync<FilmsResponse<MonetizationInfo>>(path);
        }

        public async Task<FilmsResponse<Season>> GetSeasonsDataByIdAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SeasonsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<Season>>(path);
        }

        public async Task<FilmsResponseWithPagesCount<ImageResponse>> GetImagesByIdAsync(int id,
            ImageType type = ImageType.STILL, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["type"] = type.ToString(),
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ImagesPathSegment}";
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<ImageResponse>>(path, queryParams);
        }

        public async Task<StaffResponse[]> GetStaffByFilmIdAsync(int filmId)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["filmId"] = filmId.ToString(),
            };
            string path = $"{constants.StaffUrlV1}";
            return await GetResponseDataAsync<StaffResponse[]>(path, queryParams);
        }

        public async Task<FilmsResponse<FilmPremiere>> GetPremieresListAsync(int year, Months month)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
            };
            string path = $"{constants.PremieresUrlV22}";
            return await GetResponseDataAsync<FilmsResponse<FilmPremiere>>(path, queryParams);
        }

        public async Task<FilmsResponse<Video>> GetTrailersAndTeasersByIdAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.VideosPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<Video>>(path);
        }

        public async Task<FilmsResponse<Nomination>> GetAwardsByIdAsync(int id)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.AwardsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<Nomination>>(path);
        }

        public async Task<FilmsResponse<FilmPersonInfo>> GetPersonByNameAsync(string name, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["name"] = name,
                ["page"] = page.ToString()
            };
            string path = $"{constants.PersonsUrlV1}";
            return await GetResponseDataAsync<FilmsResponse<FilmPersonInfo>>(path, queryParams);
        }

        public async Task<PersonResponse> GetStaffByPersonIdAsync(int personId)
        {
            string path = $"{constants.StaffUrlV1}/{personId}";
            return await GetResponseDataAsync<PersonResponse>(path);
        }

        protected override string GetUrl(string path, Dictionary<string, string>? queryParams)
        {
            return UrlHelper.GetUrl(path, queryParams!);
        }

        protected override async Task<HttpResponseMessage> GetResponse(string url)
        {
            return await httpClient.GetAsync(url);
        }

        protected override void ValidateResponse(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
               HttpInvalidCodeHandler.ThrowException(response.StatusCode);
        }

        protected override async Task<T> Deserialize<T>(string json)
        {
            return serializer.Deserialize<T>(json);
        }
    }
}
