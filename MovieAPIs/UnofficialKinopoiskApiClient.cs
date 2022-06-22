using System.Net.Http.Headers;
using System.Text.Json;
using MovieAPIs.ResponseModels;
using MovieAPIs.Utils;
using System.Text;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient
    {
        const string basePathSegment = "https://kinopoiskapiunofficial.tech/api";
        const string filmsPathSegment = "films";
        const string filtersPathSegment = "filters";
        const string searchByKeywordPathSegment = "search-by-keyword";
        const string similarsPathSegment = "similars";
        const string topPathSegment = "top";
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


        public async Task<FilmSearchResponse> GetFullListFilmsByKeywordAsync(string keyword)
        {
            string apiVersion = "v2.1";

            var firstPage = await GetFilmsByKeywordAsync(keyword);

            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = "1"
            };

            var urlPathsWithQuery = UrlHelper.GetPathWithQuery(queryParams, firstPage.PagesCount, basePathSegment, apiVersion, filmsPathSegment, searchByKeywordPathSegment);

            var tasks = new List<Task<string>>();

            foreach (var urlPathWithQuery in urlPathsWithQuery)
            {
                tasks.Add(client.GetStringAsync(urlPathWithQuery));
            }

            Task.WaitAll(tasks.ToArray());

            var data = new List<string>();

            foreach (var task in tasks)
            {
                data.Add(await task);
            }

            var filmsResponse = JsonSerializer.Deserialize<FilmSearchResponse>(String.Join("", data.ToArray()));

            return filmsResponse;
        }

        public async Task<FilmTopResponse> GetFullListTopFilmsAsync(Tops topType = Tops.TOP_250_BEST_FILMS)
        {
            string apiVersion = "v2.2";
            var firstPage = await GetTopFilmsAsync(topType);
            var data = new List<string>();

            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = "1"
            };

            var urlPathsWithQuery = UrlHelper.GetPathWithQuery(queryParams, firstPage.PagesCount, basePathSegment, apiVersion, filmsPathSegment, topPathSegment);

            var tasks = MultipleRequests(urlPathsWithQuery);
            
            foreach (var task in tasks)
            {
                data.Add(await task);             
            }

            var listOfFilms = new List<FilmTopResponse>();

            foreach(var curData in data)
            {
                listOfFilms.Add(JsonSerializer.Deserialize<FilmTopResponse>(curData, jsonSerializerOptions));
            }

            return null;
        }

        private List<Task<string>> MultipleRequests(string[] urlPathsWithQuery)
        {
            var tasks = new List<Task<string>>();

            var delay = 200;

            foreach (var urlPathWithQuery in urlPathsWithQuery)
            {
                tasks.Add(client.GetStringAsync(urlPathWithQuery));

                Thread.Sleep(delay);
            }

            Task.WaitAll(tasks.ToArray());

            return tasks;
        }
    }
}