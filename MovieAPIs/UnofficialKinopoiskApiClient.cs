using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using MovieAPIs.ResponseModels;
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
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            serializer = new DotNetJsonSerializer(jsonSerializerOptions);
            configuration = new JsonConfiguration(Path.Combine("Configuration", "configuration.json"));
        }
        public async Task<Film> GetFilmByIdAsync(int id)
        {
            string filmsUrl = configuration["unofficialKinopoisk:v22:filmsUrl"];
            string urlPath = UrlHelper.GetPath(filmsUrl, id.ToString());
            var responceBody = await client.GetStringAsync(urlPath);
            var film = serializer.Deserialize<Film>(responceBody);
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
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmSearchResponse>(responceBody);
            return filmsResponse;
        }

        public async Task<GenresAndCountriesSearchResponse> GetGenresAndCountriesAsync()
        {
            string genresAndCountriesUrl = configuration["unofficialKinopoisk:v22:filtersUrl"];
            var responceBody = await client.GetStringAsync(genresAndCountriesUrl);
            var genresAndCountriesId = serializer.Deserialize<GenresAndCountriesSearchResponse>(responceBody);
            return genresAndCountriesId;
        }

        public async Task<FilmSearchByFilterResponse> GetFilmsByFiltersAsync(int countryId = (int)Filter.ALL, int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
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
            string filmsUrl = configuration["unofficialKinopoisk:v22:filmsUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, filmsUrl);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmSearchByFilterResponse>(responceBody);
            return filmsResponse;
        }
        public async Task<RelatedFilmsResponce> GetRelatedFilmsAsync(int id)
        {
            string filmsUrl = configuration["unofficialKinopoisk:v22:filmsUrl"];
            string similarsPathSegment = configuration["unofficialKinopoisk:similarsPathSegment"];
            string urlPath = UrlHelper.GetPath(filmsUrl, id.ToString(), similarsPathSegment);
            var responceBody = await client.GetStringAsync(urlPath);
            var filmsResponce = serializer.Deserialize<RelatedFilmsResponce>(responceBody);
            return filmsResponce;

        }

        public async Task<FilmTopResponse> GetTopFilmsAsync(Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            string topFilmsUrl = configuration["unofficialKinopoisk:v22:topUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, topFilmsUrl);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmTopResponse>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmFactsAndMistakesResponse> GetFilmFactsAndMistakesAsync(int id)
        {
            string filmsUrl = configuration["unofficialKinopoisk:v22:filmsUrl"];
            string factsAndMistakesPathSegment = configuration["unofficialKinopoisk:factsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), factsAndMistakesPathSegment);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmFactsAndMistakesResponse>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmDistributionsResponse> GetFilmDistributionsAsync(int id)
        {
            string filmsUrl = configuration["unofficialKinopoisk:v22:filmsUrl"];
            string distributionsPathSegment = configuration["unofficialKinopoisk:distributionsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), distributionsPathSegment);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmDistributionsResponse>(responceBody);
            return filmsResponse;
        }
    }
}
