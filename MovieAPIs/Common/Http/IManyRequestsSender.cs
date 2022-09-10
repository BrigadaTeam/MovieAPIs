using MovieAPIs.Common.Helper;
using MovieAPIs.Common.Serialization;
using MovieAPIs.UnofficialKinopoiskApi.Models;
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
        internal ManyRequestsSender(IHttpClient httpClient, ISerializer serializer)
        {
            this.httpClient = httpClient;
            this.serializer = serializer;
        }
        public async IAsyncEnumerable<T> GetDataAsync<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage, [EnumeratorCancellation] CancellationToken ct)
        {
            var urls = UrlHelper.GetUrls(queryParams, path, fromPage, toPage);

            await foreach (var response in GetResponsesAsync(urls, requestCountInSecond, ct).ConfigureAwait(false))
            {
                ct.ThrowIfCancellationRequested();
                var responseBody = await response.ReadAsStringContentOrThrowExceptionAsync(ct).ConfigureAwait(false);
                var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<T>>(responseBody);
                foreach (var item in filmsResponse.Films)
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
