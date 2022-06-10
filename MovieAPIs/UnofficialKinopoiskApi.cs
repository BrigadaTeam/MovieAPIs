using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApi
    {
        const string baseUrl = "https://kinopoiskapiunofficial.tech/api/v2.2";
        const string films = "films";
        readonly HttpClient client;
        readonly JsonSerializerOptions jsonSerializerOptions;

        public UnofficialKinopoiskApi(string token)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", token);
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        public async Task<Film> GetFilmByIdAsync(int id)
        {
            string requestUrl = $"{baseUrl}/{films}/{id}";
            var responceBody = await client.GetStreamAsync(requestUrl);
            var film = JsonSerializer.Deserialize<Film>(responceBody, jsonSerializerOptions);
            return film;
        }
    }
}