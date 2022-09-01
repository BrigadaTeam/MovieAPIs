using MovieAPIs.Models;
using MovieAPIs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        protected async Task<T> GetResponseDataAsync<T>(string path, Dictionary<string, string>? queryParams = null)
        {
            string url = UrlHelper.GetUrl(path, queryParams);
            HttpResponseMessage response = await httpClient.GetAsync(url).ConfigureAwait(false);
            string json = await response.ReadAsStringContentOrThrowExceptionAsync().ConfigureAwait(false);
            return serializer.Deserialize<T>(json);
        }

        protected async IAsyncEnumerable<T> GetResponsesDataFromPageRangeAsync<T>(string path, Dictionary<string, string> queryParams, int requestCountInSecond, int fromPage, int toPage)
        {
            await foreach (var data in manyRequestsHelper.GetDataAsync<T>(queryParams, requestCountInSecond, path, fromPage, toPage).ConfigureAwait(false))
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
