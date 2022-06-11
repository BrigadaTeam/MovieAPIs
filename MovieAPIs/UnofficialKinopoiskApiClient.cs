using System.Net.Http.Headers;
using System.Text.Json;
using MovieAPIs.ResponseModels;
using MovieAPIs.Utils;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient
    {
        const string baseApiUrl = "https://kinopoiskapiunofficial.tech/api";
        const string films = "films";
        const string searchByKeyword = "search-by-keyword";
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
            string requestUrl = $"{baseApiUrl}/{apiVersion}/{films}/{id}";
            var responceBody = await client.GetStreamAsync(requestUrl);
            var film = JsonSerializer.Deserialize<Film>(responceBody, jsonSerializerOptions);
            return film;
        }
        public async Task<FilmSearchResponse> GetFilmsByKeywordAsync(string keyword, int page = 1)
        {
            string apiVersion = "v2.1";
            string requestUrl = $"{baseApiUrl}/{apiVersion}/{films}/{searchByKeyword}";
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            }; 
            var responceBody = await client.GetStreamWithQueryAsync(requestUrl, queryParams);
            var filmsResponse = JsonSerializer.Deserialize<FilmSearchResponse>(responceBody, jsonSerializerOptions);
            return filmsResponse;
        }
    }
}