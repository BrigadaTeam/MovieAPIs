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
        public async Task<RelatedFilmsResponce> GetRelatedFilmsAsync(int id)
        {
            string apiVersion = "v2.2";
            string urlPath = UrlHelper.GetPath(basePathSegment, apiVersion, filmsPathSegment, id.ToString(), similarsPathSegment);
            var responceBody = await client.GetStreamAsync(urlPath);
            var filmsResponce = JsonSerializer.Deserialize<RelatedFilmsResponce>(responceBody, jsonSerializerOptions);
            return filmsResponce;
        }


        public async Task<FilmSearchResponse> GetFullListFilmsByKeywordAsync(string keyword)
        {
            string apiVersion = "v2.1";
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = ""
            };

            var firstPage = await GetFilmsByKeywordAsync(keyword);
            var urlPathsWithQuery = new string[firstPage.PagesCount];

            for (int i = 2; i <= firstPage.PagesCount; i++)
            {
                var qParams = new Dictionary<string, string>
                {
                    ["keyword"] = keyword,
                    ["page"] = i.ToString()
                };

                urlPathsWithQuery[i] = UrlHelper.GetPathWithQuery(qParams, basePathSegment, apiVersion, filmsPathSegment, searchByKeywordPathSegment);
            }

            var tasks = new List<Task<string>>();

            foreach (var urlPathWithQuery in urlPathsWithQuery)
            {
                tasks.Add(client.GetStringAsync(urlPathWithQuery));
            }

            var data = new List<string>();

            foreach (var task in tasks)
            {
                data.Add(await task);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(data.);
            Stream stream = new MemoryStream(data);
            var filmsResponse = JsonSerializer.DeserializeAsync<FilmSearchResponse>(data);

            return filmsResponse;
        }


    }
}