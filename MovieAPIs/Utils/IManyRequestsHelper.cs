using MovieAPIs.Configuration;
using MovieAPIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.Utils
{
    internal interface IManyRequestsHelper
    {
        public IAsyncEnumerable<T> GetDataAsync<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage);
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

        public async IAsyncEnumerable<T> GetDataAsync<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage)
        {
            var urls = UrlHelper.GetUrls(queryParams, path, fromPage, toPage);
            await foreach (var response in GetResponsesAsync(urls, requestCountInSecond).ConfigureAwait(false))
            {
                var responseBody = await response.ReadAsStringContentOrThrowExceptionAsync().ConfigureAwait(false);
                var filmsResponse = serializer.Deserialize<FilmsResponseWithPagesCount<T>>(responseBody);
                foreach (var item in filmsResponse.Films)
                {
                    yield return item;
                }
            }
        }

        async IAsyncEnumerable<HttpResponseMessage> GetResponsesAsync(IEnumerable<string> requestUrls, int requestCountInSecond)
        {
            int index = 0;
            int count = requestUrls.Count();
            while (index < count)
            {
                var timer = Task.Delay(TimeSpan.FromSeconds(1));
                var tasks = requestUrls.Skip(index).Take(requestCountInSecond).Select(x => httpClient.GetAsync(x));
                var tasksAndTimer = tasks.Concat(new Task[] { timer });
                await Task.WhenAll(tasksAndTimer).ConfigureAwait(false);
                foreach (var task in tasks)
                {
                    yield return await task.ConfigureAwait(false);
                }
                index += requestCountInSecond;
            }
        }
    }
}
