using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MovieAPIs.Common.Serialization;
using MovieAPIs.Common.Helper;
using MovieAPIs.Common.Http;
using MovieAPIs.Common.Responses;

namespace MovieAPIs.Common
{
    /// <summary>
    /// Base class for Movie api client.
    /// </summary>
    public abstract class MovieApiClientBase
    {
        /// <summary>
        /// Provides a class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
        /// </summary>
        readonly IHttpClient httpClient;
        
        /// <summary>
        /// Provides classes that are used when serializing and deserializing JSON.
        /// </summary>
        readonly ISerializer serializer;
        
        /// <summary>
        /// Provides classes that are used when multiple sending Http receipts and receiving data from their responses.
        /// </summary>
        readonly IManyRequestsSender manyRequestsHelper;
        
        /// <summary>
        /// Class for catching exceptions by http status code.
        /// </summary>
        readonly HttpInvalidCodeHandler httpInvalidCodeHandler;

        /// <summary>
        /// Movie api client base constructor that accepts custom parameters.
        /// </summary>
        /// <param name="httpClient">Sending HTTP requests and receiving HTTP responses.</param>
        /// <param name="httpInvalidCodeHandler">Catching exceptions by http status code.</param>
        internal MovieApiClientBase(IHttpClient httpClient, HttpInvalidCodeHandler httpInvalidCodeHandler)
        {
            serializer = new NewtonsoftJsonSerializer();
            this.httpClient = httpClient;
            manyRequestsHelper = new ManyRequestsSender(httpClient, serializer, httpInvalidCodeHandler);
            this.httpInvalidCodeHandler = httpInvalidCodeHandler;
        }
        
        /// <summary>
        /// Request to api to get data.
        /// </summary>
        /// <param name="path">Contains protocol, host and resource path.</param>
        /// <param name="queryParams">Http request parameters.</param>
        /// <typeparam name="T">Response models.</typeparam>
        /// <returns>A task object with a response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
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

        /// <summary>
        /// Multiple request to api to get data on pages.
        /// </summary>
        /// <param name="path">Contains protocol, host and resource path.</param>
        /// <param name="queryParams">Http request parameters.</param>
        /// <param name="requestCountInSecond">Requests per second limit.</param>
        /// <param name="fromPage">Start page of the answer response.</param>
        /// <param name="toPage">End page of the answer response.</param>
        /// <param name="firstPage">First page of the answer response</param>
        /// <param name="response">Response containing the number of pages.</param>
        /// <typeparam name="T">Response models.</typeparam>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of response models.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        protected async IAsyncEnumerable<T> GetResponsesDataFromPageRangeAsync<T>(string path, Dictionary<string, string> queryParams, int requestCountInSecond, int fromPage, int toPage, int firstPage, int pagesCount, [EnumeratorCancellation] CancellationToken ct = default)
        {
            fromPage = fromPage == -1 ? firstPage : fromPage;
            toPage = toPage == -1 ? pagesCount : toPage;
            await foreach (var data in manyRequestsHelper.GetDataAsync<T>(queryParams, requestCountInSecond, path, fromPage, toPage, ct).ConfigureAwait(false))
            {
                yield return data;
            }
        }
    }
}
