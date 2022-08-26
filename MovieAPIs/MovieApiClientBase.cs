using MovieAPIs.Utils;
using MovieAPIs.Models;
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
        readonly IManyRequestsHelper manyRequestsHelper;
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
            string jsonResponse = await response.ReadAsStringContentOrThrowExceptionAsync(ct);
            ct.ThrowIfCancellationRequested();
            return serializer.Deserialize<T>(jsonResponse);
        }

        protected async IAsyncEnumerable<T> GetResponsesDataFromPageRangeAsync<T>(string path, Dictionary<string, string> queryParams, int requestCountInSecond, int fromPage, int toPage, CancellationToken ct)
        {
            var filmsResponses = manyRequestsHelper.GetData<T>(queryParams, requestCountInSecond, path, fromPage, toPage, ct);
            await foreach (var filmsResponse in filmsResponses)
            {
                yield return filmsResponse;
            }
        }

        protected int GetPagesCount<T>(Task<FilmsResponseWithPagesCount<T>> response)
        {
            return response.Result.PagesCount;
        }
    }
}
