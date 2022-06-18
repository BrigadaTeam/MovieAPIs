using System.Net.Http.Headers;
using System.Text.Json;
using MovieAPIs.ResponseModels;
using MovieAPIs.Utils;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient
    {
        const string basePathSegment = "https://kinopoiskapiunofficial.tech/api";
        const string filmsPathSegment = "films";
        const string filtersPathSegment = "filters";
        const string searchByKeywordPathSegment = "search-by-keyword";
        const string similarsPathSegment = "similars";
        readonly HttpClient client;
        readonly JsonSerializerOptions jsonSerializerOptions;

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
        }
        public async Task<Film> GetFilmByIdAsync(int id)
        {
            string apiVersion = "v2.2";
            string urlPath = UrlHelper.GetPath(basePathSegment, apiVersion, filmsPathSegment, id.ToString());
            var responceBody = await client.GetStreamAsync(urlPath);
            var film = JsonSerializer.Deserialize<Film>(responceBody, jsonSerializerOptions);
            return film;
        }
        public async Task<FilmSearchResponse> GetFilmsByKeywordAsync(string keyword, int page = 1)
        {
            string apiVersion = "v2.1";
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, basePathSegment, apiVersion, filmsPathSegment, searchByKeywordPathSegment);
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
            string apiVersion = "v2.2";
            string urlPath = UrlHelper.GetPath(basePathSegment, apiVersion, filmsPathSegment, id.ToString(), similarsPathSegment);
            var responceBody = await client.GetStreamAsync(urlPath);
            var filmsResponce = JsonSerializer.Deserialize<RelatedFilmsResponce>(responceBody, jsonSerializerOptions);
            return filmsResponce;

        }
    }
}