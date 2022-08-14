using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MovieAPIs.Models;
using MovieAPIs.Utils;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient
    {
        readonly IHttpClient httpClient;
        readonly IConfiguration configuration;
        readonly ISerializer serializer;
        readonly IManyRequestsHelper manyRequestsHelper;

        public UnofficialKinopoiskApiClient(string apiKey) : this(new InternalHttpClient(apiKey)) { }

        internal UnofficialKinopoiskApiClient(IHttpClient httpClient)
        {
            configuration = new JsonConfiguration(Path.Combine("Configuration", "configuration.json"));
            serializer = new NewtonsoftJsonSerializer();
            this.httpClient = httpClient;
            manyRequestsHelper = new ManyRequestsHelper(httpClient, serializer);
        }

        public async Task<Film> GetFilmByIdAsync(int id)
        {          
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var urlPath = UrlHelper.GetPath(filmsUrl, id.ToString());

            var response = await httpClient.GetAsync(urlPath);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var film = serializer.Deserialize<Film>(responseBody);
            return film;
        }
        public async IAsyncEnumerable<FilmSearch> GetFilmsByKeywordFromAllPagesAsync(string keyword)
        {
            var firstFilmsResponse = await GetFilmsByKeywordAsync(keyword);
            int pagesCount = firstFilmsResponse.PagesCount;
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword
            };
            var searchByKeywordUrl = configuration["UnofficialKinopoisk:V21:SearchByKeywordUrl"];
            var filmsResponses = manyRequestsHelper.GetDataFromAllPages<FilmSearch>(queryParams, pagesCount, 5, searchByKeywordUrl);
            await foreach(var filmsResponse in filmsResponses)
            {
                yield return filmsResponse;
            }
        }

        public async IAsyncEnumerable<FilmSearch> GetTopFilmsFromAllPagesAsync(Tops topType = Tops.TOP_250_BEST_FILMS)
        {
            var firstFilmsResponse = await GetTopFilmsAsync(topType);
            int pagesCount = firstFilmsResponse.PagesCount;
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString()            };
            var topFilmsUrl = configuration["UnofficialKinopoisk:V22:TopUrl"];
            var filmsResponses = manyRequestsHelper.GetDataFromAllPages<FilmSearch>(queryParams, pagesCount, 5, topFilmsUrl);
            await foreach (var filmsResponse in filmsResponses)
            {
                yield return filmsResponse;
            }
        }
        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetFilmsByKeywordAsync(string keyword, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            var searchByKeywordUrl = configuration["UnofficialKinopoisk:V21:SearchByKeywordUrl"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, searchByKeywordUrl);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<FilmSearch>>(responseBody);
            return filmsResponse;
        }

        public async Task<GenresAndCountriesResponse> GetGenresAndCountriesAsync()
        {
            var genresAndCountriesUrl = configuration["UnofficialKinopoisk:V22:FiltersUrl"];
            var response = await httpClient.GetAsync(genresAndCountriesUrl);
            if (!response.IsSuccessStatusCode)               
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var genresAndCountriesId = serializer.Deserialize<GenresAndCountriesResponse>(responseBody);
            return genresAndCountriesId;
        }

        public async Task<FilmsResponseWithPagesCount<Film>> GetFilmsByFiltersAsync(int countryId = (int)Filter.ALL, int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
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
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, filmsUrl);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<Film>>(responseBody);
            return filmsResponse;
        }
        
        public async Task<FilmsResponse<RelatedFilm>> GetRelatedFilmsAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var similarsPathSegment = configuration["UnofficialKinopoisk:SimilarsPathSegment"];
            var urlPath = UrlHelper.GetPath(filmsUrl, id.ToString(), similarsPathSegment);
            var response = await httpClient.GetAsync(urlPath);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<RelatedFilm>>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetTopFilmsAsync(Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            var topFilmsUrl = configuration["UnofficialKinopoisk:V22:TopUrl"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, topFilmsUrl);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<FilmSearch>>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<FactsAndMistakes>> GetFilmFactsAndMistakesAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var factsAndMistakesPathSegment = configuration["UnofficialKinopoisk:FactsPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), factsAndMistakesPathSegment);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<FactsAndMistakes>>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<FilmDistributionsResponseItems>> GetFilmDistributionsAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var distributionsPathSegment = configuration["UnofficialKinopoisk:DistributionsPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), distributionsPathSegment);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<FilmDistributionsResponseItems>>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponseWithPagesCount<FilmRelease>> GetDigitalReleasesAsync(int year, Months month, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
                ["page"] = page.ToString()
            };
            var releaseFilmsUrl = configuration["UnofficialKinopoisk:V21:ReleasesUrl"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, releaseFilmsUrl);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<FilmRelease>>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmSearch[]> GetSequelsAndPrequelsByIdAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V21:FilmsUrl"];
            var sequelsAndPrequelsPathSegment = configuration["UnofficialKinopoisk:SequelsAndPrequelsPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), sequelsAndPrequelsPathSegment);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmSearch[]>(responseBody);
            return filmsResponse;
        }

        public async Task<ViewerReviewsResponse> GetViewerReviewsByIdAsync(int id, int page = 1, ReviewOrder order = ReviewOrder.DATE_DESC)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["order"] = order.ToString(),
            };
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var viewerReviewsPathSegment = configuration["UnofficialKinopoisk:ReviewsPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, UrlHelper.GetPath(filmsUrl, id.ToString(), viewerReviewsPathSegment));
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<ViewerReviewsResponse>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<MonetizationInfo>> GetBoxOfficeByIdAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var boxOfficePathSegment = configuration["UnofficialKinopoisk:BoxOfficePathSegment"];
            var urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), boxOfficePathSegment);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<MonetizationInfo>>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<Season>> GetSeasonsDataByIdAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var seasonsPathSegment = configuration["UnofficialKinopoisk:SeasonsPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), seasonsPathSegment);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<Season>>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponseWithPagesCount<ImageResponse>> GetImagesByIdAsync(int id,
            ImageType type = ImageType.STILL, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["type"] = type.ToString(),
            };
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var imagePathSegment = configuration["UnofficialKinopoisk:ImagesPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams,
                    UrlHelper.GetPath(filmsUrl, id.ToString(), imagePathSegment));
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<ImageResponse>>(responseBody);
            return filmsResponse;
        }

        public async Task<StaffResponse[]> GetStaffByFilmIdAsync(int filmId)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["filmId"] = filmId.ToString(),
            };
            var staffUrl = configuration["UnofficialKinopoisk:V1:StaffUrl"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, staffUrl);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<StaffResponse[]>(responseBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<FilmPremiere>> GetPremieresListAsync(int year, Months month)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
            };
            var premiereUrl = configuration["UnofficialKinopoisk:V22:PremieresUrl"];
            var urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, premiereUrl);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode)
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<FilmPremiere>>(responseBody);
            return filmsResponse;
        }
        
        public async Task<FilmsResponse<Video>> GetTrailersAndTeasersByIdAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var videoPathSegment = configuration["UnofficialKinopoisk:VideosPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), videoPathSegment);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<Video>>(responseBody);
            return filmsResponse;
        }
        
        public async Task<FilmsResponse<Nomination>> GetAwardsByIdAsync(int id)
        {
            var filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            var awardsPathSegment = configuration["UnofficialKinopoisk:AwardsPathSegment"];
            var urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), awardsPathSegment);
            var response = await httpClient.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) 
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<Nomination>>(responseBody);
            return filmsResponse;
        }
    }
}
