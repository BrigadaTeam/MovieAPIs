using MovieAPIs.Common.Helper;
using MovieAPIs.Common.Responses;
using MovieAPIs.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Common.Http
{
    internal interface IManyRequestsSender
    {
        public IAsyncEnumerable<T> GetDataAsync<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage, CancellationToken ct);
    }
    internal class ManyRequestsSender : IManyRequestsSender
    {
        readonly IHttpClient httpClient;
        readonly ISerializer serializer;
        readonly HttpInvalidCodeHandler httpInvalidCodeHandler;
        internal ManyRequestsSender(IHttpClient httpClient, ISerializer serializer, HttpInvalidCodeHandler httpInvalidCodeHandler)
        {
            this.httpClient = httpClient;
            this.serializer = serializer;
            this.httpInvalidCodeHandler = httpInvalidCodeHandler;
        }
        public async IAsyncEnumerable<T> GetDataAsync<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage, [EnumeratorCancellation] CancellationToken ct)
        {
            var urls = UrlHelper.GetUrls(queryParams, path, fromPage, toPage);

            await foreach (var response in GetResponsesAsync(urls, requestCountInSecond, ct).ConfigureAwait(false))
            {
                ct.ThrowIfCancellationRequested();
                if (!response.IsSuccessStatusCode)
                    httpInvalidCodeHandler.ThrowException(response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                var filmsResponse = serializer.Deserialize<ItemsResponseWithPagesCount<T>>(responseBody);
                foreach (var item in filmsResponse.Items)
                {
                    yield return item;
                }
            }
        }

        async IAsyncEnumerable<HttpResponseMessage> GetResponsesAsync(IEnumerable<string> requestUrls, int requestCountInSecond, [EnumeratorCancellation] CancellationToken ct)
        {
            int index = 0;
            int count = requestUrls.Count();
            while (index < count)
            {
                var timer = Task.Delay(TimeSpan.FromSeconds(1), ct);
                var tasks = requestUrls.Skip(index).Take(requestCountInSecond).Select(x => httpClient.GetAsync(x, ct));
                var tasksAndTimer = tasks.Concat(new[] { timer });
                await Task.WhenAll(tasksAndTimer).ConfigureAwait(false);
                foreach (var task in tasks)
                {
                    ct.ThrowIfCancellationRequested();
                    yield return await task.ConfigureAwait(false);
                }
                index += requestCountInSecond;
            }
        }
    }
}
