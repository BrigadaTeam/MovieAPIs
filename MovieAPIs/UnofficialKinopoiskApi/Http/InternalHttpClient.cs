using MovieAPIs.Common.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.UnofficialKinopoiskApi.Http
{
    internal class InternalHttpClient : IHttpClient
    {
        HttpClient client;
        public InternalHttpClient(string apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken ct = default)
        {
            return await client.GetAsync(requestUrl, ct).ConfigureAwait(false);
        }
    }
}
