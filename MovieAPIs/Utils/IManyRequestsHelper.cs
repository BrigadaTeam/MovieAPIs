using MovieAPIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Utils
{
    internal interface IManyRequestsHelper
    {
        public IAsyncEnumerable<T> GetData<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage, CancellationToken ct);
    }
    internal class ManyRequestsHelper : IManyRequestsHelper
    {
        readonly IHttpClient httpClient;
        readonly ISerializer serializer;
        internal ManyRequestsHelper(IHttpClient httpClient, ISerializer serializer)
        {
            this.httpClient = httpClient;
            this.serializer = serializer;
        }
        public async IAsyncEnumerable<T> GetData<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage, [EnumeratorCancellation] CancellationToken ct)
        {
            var urls = UrlHelper.GetUrls(queryParams, path, fromPage, toPage);
            var responses = GetResponsesAsync(urls, requestCountInSecond, ct);
            await foreach (var response in responses.WithCancellation(ct))
            {
                var responseBody = await response.ReadAsStringContentOrThrowExceptionAsync(ct);
                var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<T>>(responseBody);
                foreach (var item in filmsResponse.Films)
                {
                    yield return item;
                }
            }
        }

        async IAsyncEnumerable<HttpResponseMessage> GetResponsesAsync(IEnumerable<string> requestUrls, int requestCountInSecond, [EnumeratorCancellation] CancellationToken ct)
        {
            var index = 0;
            var urls = requestUrls as string[] ?? requestUrls.ToArray();
            var count = urls.Count();
            while (index < count)
            {
                var tasks = urls.Skip(index).Take(requestCountInSecond).Select(x => httpClient.GetAsync(x, ct));
                var enumerable = tasks as Task<HttpResponseMessage>[] ?? tasks.ToArray();
                var timer = Task.Delay(TimeSpan.FromSeconds(1), ct);
                var tasksAndTimer = enumerable.Concat(new[] { timer });
                await Task.WhenAll(tasksAndTimer);
                foreach (var task in enumerable)
                {
                    yield return await task;
                }
                index += requestCountInSecond;
            }
        }
    }
}
