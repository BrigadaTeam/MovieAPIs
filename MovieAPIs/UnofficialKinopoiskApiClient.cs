using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieAPIs.Models;
using MovieAPIs.Utils;
using MovieAPIs.Models.Responses;

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
            var responceBody = await client.GetStringAsync(urlPath);
            var film = serializer.Deserialize<Film>(responceBody);
            return film;
        }
        public async Task<FilmsResponceWithPageCount<FilmSearch>> GetFilmsByKeywordAsync(string keyword, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            string searchByKeywordUrl = configuration["UnofficialKinopoisk:V21:SearchByKeywordUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, searchByKeywordUrl);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmsResponceWithPageCount<FilmSearch>>(responceBody);
            return filmsResponse;
        }

        public async Task<GenresAndCountriesResponse> GetGenresAndCountriesAsync()
        {
            string genresAndCountriesUrl = configuration["UnofficialKinopoisk:V22:FiltersUrl"];
            var responceBody = await client.GetStringAsync(genresAndCountriesUrl);
            var genresAndCountriesId = serializer.Deserialize<GenresAndCountriesResponse>(responceBody);
            return genresAndCountriesId;
        }

        public async Task<FilmsResponceWithPageCount<Film>> GetFilmsByFiltersAsync(int countryId = (int)Filter.ALL, int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
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
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmsResponceWithPageCount<Film>>(responceBody);
            return filmsResponse;
        }
        public async Task<FilmsResponse<RelatedFilm>> GetRelatedFilmsAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string similarsPathSegment = configuration["UnofficialKinopoisk:SimilarsPathSegment"];
            string urlPath = UrlHelper.GetPath(filmsUrl, id.ToString(), similarsPathSegment);
            var responceBody = await client.GetStringAsync(urlPath);
            var filmsResponce = serializer.Deserialize<FilmsResponse<RelatedFilm>>(responceBody);
            return filmsResponce;

        }

        public async Task<FilmsResponceWithPageCount<FilmSearch>> GetTopFilmsAsync(Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            string topFilmsUrl = configuration["UnofficialKinopoisk:V22:TopUrl"];
            string urlPathWithQuery = UrlHelper.GetPathWithQuery(queryParams, topFilmsUrl);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmsResponceWithPageCount<FilmSearch>>(responceBody);
            return filmsResponse;
        }

        public async Task<FilmsResponse<FilmDistributionsResponseItems>> GetFilmDistributionsAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string distributionsPathSegment = configuration["UnofficialKinopoisk:DistributionsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), distributionsPathSegment);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<FilmsResponse<FilmDistributionsResponseItems>>(responceBody);
            return filmsResponse;
        }

        public async Task<SerialResponse> GetSeasonDataByIdAsync(int id)
        {
            string filmsUrl = configuration["UnofficialKinopoisk:V22:FilmsUrl"];
            string seasonsPathSegment = configuration["UnofficialKinopoisk:SeasonsPathSegment"];
            string urlPathWithQuery = UrlHelper.GetPath(filmsUrl, id.ToString(), seasonsPathSegment);
            var responceBody = await client.GetStringAsync(urlPathWithQuery);
            var filmsResponse = serializer.Deserialize<SerialResponse>(responceBody);
            return filmsResponse;
        }

    }
}
