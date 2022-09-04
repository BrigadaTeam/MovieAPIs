using MovieAPIs.Utils;
using MovieAPIs.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs
{
    public abstract class MovieApiClientBase
    {
        readonly IHttpClient httpClient;
        readonly ISerializer serializer;
        readonly IManyRequestsHelper manyRequestsHelper;

        public MovieApiClientBase(IHttpClient httpClient)
        {
            serializer = new NewtonsoftJsonSerializer();
            this.httpClient = httpClient;
            manyRequestsHelper = new ManyRequestsHelper(httpClient, serializer);
        }
        protected async Task<T> GetResponseDataAsync<T>(string path, CancellationToken ct = default, Dictionary<string, string>? queryParams = null)
        {
            string url = UrlHelper.GetUrl(path, queryParams);
            ct.ThrowIfCancellationRequested();
            HttpResponseMessage response = await httpClient.GetAsync(url, ct).ConfigureAwait(false);
            string json = await response.ReadAsStringContentOrThrowExceptionAsync(ct).ConfigureAwait(false);
            return serializer.Deserialize<T>(json);
        }

        protected async IAsyncEnumerable<T> GetResponsesDataFromPageRangeAsync<T>(string path, Dictionary<string, string> queryParams, int requestCountInSecond, int fromPage, int toPage, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var data in manyRequestsHelper.GetDataAsync<T>(queryParams, requestCountInSecond, path, fromPage, toPage, ct).ConfigureAwait(false))
            {
                yield return data;
            }
        }

        protected async Task<int> GetPagesCountAsync<T>(Task<FilmsResponseWithPagesCount<T>> response)
        {
            return (await response.ConfigureAwait(false)).PagesCount;
        }
    }
}
