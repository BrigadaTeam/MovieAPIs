using MovieAPIs.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs
{
    public abstract class MovieApiClientBase
    {
        readonly IHttpClient httpClient;
        readonly ISerializer serializer;
        internal readonly IManyRequestsHelper manyRequestsHelper;
        public MovieApiClientBase(IHttpClient httpClient, ISerializer serializer)
        {
            this.serializer = serializer;
            this.httpClient = httpClient;
            manyRequestsHelper = new ManyRequestsHelper(httpClient, serializer);
        }
        internal async Task<T> GetResponseDataAsync<T>(string path, CancellationToken ct, Dictionary<string, string>? queryParams = null)
        {
            var url = UrlHelper.GetUrl(path, queryParams);
            HttpResponseMessage response = await httpClient.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode)
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync(ct);
            ct.ThrowIfCancellationRequested();
            return serializer.Deserialize<T>(jsonResponse);
        }
    }
}
