using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Utils
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken ct);
    }

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
        public Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken ct)
        {
            return client.GetAsync(requestUrl, ct);
        }
    }
}
