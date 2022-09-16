using MovieAPIs.Common.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.UnofficialKinopoiskApi.Http
{
    /// <summary>
    /// Class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI to unofficial kinopoisk api.
    /// </summary>
    internal class UnofficialKinopoiskHttpClient : IHttpClient
    {
        /// <summary>
        /// Class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
        /// </summary>
        HttpClient client;
        
        /// <summary>
        /// Unofficial kinopoisk HttpClient base constructor that accepts api key as parameter.
        /// </summary>
        /// <param name="apiKey">Key for unofficial kinopoisk api</param>
        public UnofficialKinopoiskHttpClient(string apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        }
        
        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUrl">Request Url for Unofficial kinopoisk api.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>A task object that represents  a HTTP response message including the status code and data.</returns>
        public async Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken ct = default)
        {
            return await client.GetAsync(requestUrl, ct).ConfigureAwait(false);
        }
    }
}
