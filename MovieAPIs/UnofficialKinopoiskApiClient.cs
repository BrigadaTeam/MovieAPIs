using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieAPIs.Models;
using MovieAPIs.Utils;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient
    {
        readonly HttpClient client;
        readonly IConfiguration configuration;
        readonly ISerializer serializer;

        public UnofficialKinopoiskApiClient(string apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            serializer = new NewtonsoftJsonSerializer();
            configuration = new JsonConfiguration(Path.Combine("Configuration", "configuration.json"));
        }
        public async Task<Film> GetFilmByIdAsync(int id)
        {          
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string urlPath = UrlHelper.GetPath(filmsUrl, id.ToString());
            var response = await client.GetAsync(urlPath);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var film = serializer.Deserialize<Film>(responceBody);
            return film;
        }

        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetFilmsByKeywordAsync(string keyword, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            string searchByKeywordUrl = configuration["UnofficialKinopoisk:V21:SearchByKeywordUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, searchByKeywordUrl);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<FilmSearch>>(responceBody);
            return filmsResponse;
        }

        public async Task<GenresAndCountriesResponse> GetGenresAndCountriesAsync()
        {
            string genresAndCountriesUrl = configuration["UnofficialKinopoisk:V22:FiltersUrl"];
            var response = await client.GetAsync(genresAndCountriesUrl);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var genresAndCountriesId = serializer.Deserialize<GenresAndCountriesResponse>(responceBody);
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
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, filmsUrl);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<Film>>(responceBody);
            return filmsResponse;
        }
        public async Task<FilmsResponse<RelatedFilm>> GetRelatedFilmsAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string similarsPathSegment = configuration["UnofficialKinopoisk:SimilarsPathSegment"];
            string urlPath = UrlHelper.GetPath(filmsUrl, id.ToString(), similarsPathSegment);
            var response = await client.GetAsync(urlPath);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponce = serializer.Deserialize<FilmsResponse<RelatedFilm>>(responceBody);
            return filmsResponce;
        }

        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetTopFilmsAsync(Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            string topFilmsUrl = configuration["UnofficialKinopoisk:V22:TopUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, topFilmsUrl);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<FilmSearch>>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<FactsAndMistakes>> GetFilmFactsAndMistakesAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string factsAndMistakesPathSegment = configuration["UnofficialKinopoisk:FactsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), factsAndMistakesPathSegment);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<FactsAndMistakes>>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<FilmDistributionsResponseItems>> GetFilmDistributionsAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string distributionsPathSegment = configuration["UnofficialKinopoisk:DistributionsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), distributionsPathSegment);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<FilmDistributionsResponseItems>>(responceBody);
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
            string releaseFilmsUrl = configuration["UnofficialKinopoisk:V21:ReleasesUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, releaseFilmsUrl);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<FilmRelease>>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmSearch[]> GetSequelsAndPrequelsByIdAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V21:FilmsUrl"];
            string sequelsAndPrequelsPathSegment = configuration["UnofficialKinopoisk:SequelsAndPrequelsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), sequelsAndPrequelsPathSegment);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmSearch[]>(responceBody);
            return filmsResponse;
        }

        public async Task<ViewerReviewsResponse> GetViewerReviewsByIdAsync(int id, int page = 1, ReviewOrder order = ReviewOrder.DATE_DESC)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["order"] = order.ToString(),
            };
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string viewerReviewsPathSegment = configuration["UnofficialKinopoisk:ReviewsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, UrlHelper.GetPath(filmsUrl, id.ToString(), viewerReviewsPathSegment));
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<ViewerReviewsResponse>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<MonetizationInfo>> GetBoxOfficeByIdAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string boxOfficePathSegment = configuration["UnofficialKinopoisk:BoxOfficePathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), boxOfficePathSegment);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<MonetizationInfo>>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<Season>> GetSeasonsDataByIdAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string seasonsPathSegment = configuration["UnofficialKinopoisk:SeasonsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), seasonsPathSegment);
            var response = await client.GetAsync(urlPathWithQuery);
            if (!response.IsSuccessStatusCode) ExceptionHandler.GetException(response.StatusCode);
            var responceBody = await response.Content.ReadAsStringAsync();
            var filmsResponse = serializer.Deserialize<FilmsResponse<Season>>(responceBody);
            return filmsResponse;
        }
    }
}
