using System.Net.Http.Headers;
using System.Text.Json;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient
    {
        const string baseApiUrl = "https://kinopoiskapiunofficial.tech/api";
        const string films = "films";
        const string lastApiVersion = "v2.2";
        readonly HttpClient client;
        readonly JsonSerializerOptions jsonSerializerOptions;
        readonly string apiVersion;

        public UnofficialKinopoiskApiClient(string apiKey, string apiVersion)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            this.apiVersion = apiVersion;
        }
        public UnofficialKinopoiskApiClient(string apiKey)
            : this(apiKey, lastApiVersion) { }
        public async Task<Film> GetFilmByIdAsync(int id)
        {
            string requestUrl = $"{baseApiUrl}/{apiVersion}/{films}/{id}";
            var responceBody = await client.GetStreamAsync(requestUrl);
            var film = JsonSerializer.Deserialize<Film>(responceBody, jsonSerializerOptions);
            return film;
        }
    }
}