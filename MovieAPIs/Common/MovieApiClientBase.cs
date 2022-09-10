using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MovieAPIs.Common.Serialization;
using MovieAPIs.Common.Helper;
using MovieAPIs.Common.Http;
using MovieAPIs.Common.Responses;
using System;

namespace MovieAPIs.Common
{
    public abstract class MovieApiClientBase
    {
        readonly IHttpClient httpClient;
        readonly ISerializer serializer;
        readonly IManyRequestsSender manyRequestsHelper;
        readonly HttpInvalidCodeHandler httpInvalidCodeHandler;

        internal MovieApiClientBase(IHttpClient httpClient, HttpInvalidCodeHandler httpInvalidCodeHandler)
        {
            serializer = new NewtonsoftJsonSerializer();
            this.httpClient = httpClient;
            manyRequestsHelper = new ManyRequestsSender(httpClient, serializer, httpInvalidCodeHandler);
            this.httpInvalidCodeHandler = httpInvalidCodeHandler;
        }
        protected async Task<T> GetResponseDataAsync<T>(string path, CancellationToken ct = default, Dictionary<string, string>? queryParams = null)
        {
            string url = UrlHelper.GetUrl(path, queryParams);
            ct.ThrowIfCancellationRequested();
            HttpResponseMessage response = await httpClient.GetAsync(url, ct).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                httpInvalidCodeHandler.ThrowException(response.StatusCode);
            var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            return serializer.Deserialize<T>(json);
        }

        protected async IAsyncEnumerable<T> GetResponsesDataFromPageRangeAsync<T>(string path, Dictionary<string, string> queryParams, int requestCountInSecond, int fromPage, int toPage, int firstPage, ItemsResponseWithPagesCount<T> response,[EnumeratorCancellation] CancellationToken ct = default)
        {
            fromPage = fromPage == -1 ? firstPage : fromPage;
            toPage = toPage == -1 ? response.PagesCount : toPage;
            await foreach (var data in manyRequestsHelper.GetDataAsync<T>(queryParams, requestCountInSecond, path, fromPage, toPage, ct).ConfigureAwait(false))
            {
                yield return data;
            }
        }
    }
}
