using MovieAPIs.Common;
using MovieAPIs.Common.Http;
using MovieAPIs.Common.Responses;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIsTest.Utils
{
    internal class MovieApiClientTest : MovieApiClientBase
    {
        public MovieApiClientTest(IHttpClient httpClient, HttpInvalidCodeHandler httpInvalidCodeHandler) : base(httpClient, httpInvalidCodeHandler) { }

        public async Task<T> GetResponseDataAsync<T>(string path, CancellationToken ct = default, Dictionary<string, string>? queryParams = null)
        {
            return await base.GetResponseDataAsync<T>(path, ct, queryParams);
        }

        public async IAsyncEnumerable<T> GetResponsesDataFromPageRangeAsync<T>(string path, Dictionary<string, string> queryParams, int requestCountInSecond, int fromPage, int toPage, int firstPage, int pagesCount, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var data in base.GetResponsesDataFromPageRangeAsync<T>(path, queryParams, requestCountInSecond, fromPage, toPage, firstPage, pagesCount, ct))
            {
                yield return data;
            }
        }
    }
}
