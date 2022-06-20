using System.Net.Http.Headers;
using System.Text.Json;
using MovieAPIs.ResponseModels;
using MovieAPIs.Utils;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient
    {
        readonly HttpClient client;
        readonly JsonSerializerOptions jsonSerializerOptions;
        readonly IConfiguration configuration;

        public UnofficialKinopoiskApiClient(string apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            configuration = new JsonConfiguration(@"Configuration\configuration.json");
        }
        public async Task<Film> GetFilmByIdAsync(int id)
        {
            string filmsUrl = configuration["unofficialKinopoisk:v22:filmsUrl"];
            string urlPath = UrlHelper.GetPath(filmsUrl, id.ToString());
            var responceBody = await client.GetStreamAsync(urlPath);
            var film = JsonSerializer.Deserialize<Film>(responceBody, jsonSerializerOptions);
            return film;
        }
        public async Task<FilmSearchResponse> GetFilmsByKeywordAsync(string keyword, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            string searchByKeywordUrl = configuration["unofficialKinopoisk:v21:searchByKeywordUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, searchByKeywordUrl);
            var responceBody = await client.GetStreamAsync(urlPathWithQuery);
            var filmsResponse = JsonSerializer.Deserialize<FilmSearchResponse>(responceBody, jsonSerializerOptions);
            return filmsResponse;
        }

        public async Task<GenresAndCountriesSearchResponse> GetGenresAndCountriesAsync()
        {
            string apiVersion = "v2.2";

            string urlPath = UrlHelper.GetPath(basePathSegment, apiVersion, filmsPathSegment, filtersPathSegment);
            var responceBody = await client.GetStreamAsync(urlPath);
            var genresAndCountriesId = JsonSerializer.Deserialize<GenresAndCountriesSearchResponse>(responceBody, jsonSerializerOptions);
            return genresAndCountriesId;
        }

        public async Task<FilmSearchByFilterResponse> GetFilmsByFiltersAsync(int countryId = (int)Filter.ALL, int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
            MovieOrder order = MovieOrder.RATING, MovieType type = MovieType.ALL, int ratingFrom = 0, int ratingTo = 10,
            int yearFrom = 1000, int yearTo = 3000, int page = 1)
        {
            string apiVersion = "v2.2";

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

            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, basePathSegment, apiVersion, filmsPathSegment);
            var responceBody = await client.GetStreamAsync(urlPathWithQuery);
            var filmsResponse = JsonSerializer.Deserialize<FilmSearchByFilterResponse>(responceBody, jsonSerializerOptions);
            return filmsResponse;
        }
        public async Task<RelatedFilmsResponce> GetRelatedFilmsAsync(int id)
        {
            string filmsUrl = configuration["unofficialKinopoisk:v22:filmsUrl"];
            string similarsPathSegment = configuration["unofficialKinopoisk:similarsPathSegment"];
            string urlPath = UrlHelper.GetPath(filmsUrl, id.ToString(), similarsPathSegment);
            var responceBody = await client.GetStreamAsync(urlPath);
            var filmsResponce = JsonSerializer.Deserialize<RelatedFilmsResponce>(responceBody, jsonSerializerOptions);
            return filmsResponce;

        }

        public async Task<FilmTopResponse> GetTopFilmsAsync(Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1)
        {
            string apiVersion = "v2.2";
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, basePathSegment, apiVersion, filmsPathSegment, topPathSegment);
            var responceBody = await client.GetStreamAsync(urlPathWithQuery);
            var filmsResponse = JsonSerializer.Deserialize<FilmTopResponse>(responceBody, jsonSerializerOptions);
            return filmsResponse;
        }
    }
}
