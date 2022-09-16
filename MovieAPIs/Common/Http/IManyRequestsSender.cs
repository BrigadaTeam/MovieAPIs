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
    /// <summary>
    /// Interface for working with multiple sending Http receipts and receiving data from their responses.
    /// </summary>
    internal interface IManyRequestsSender
    {
        /// <summary>
        /// Asynchronous method for getting data from multiple requests.
        /// </summary>
        /// <param name="queryParams">Contains query parameter as a key, and its query value as a value.</param>
        /// <param name="requestCountInSecond">Requests per second limit.</param>
        /// <param name="path">Contains protocol, host and resource path.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection.</returns>
        public IAsyncEnumerable<T> GetDataAsync<T>(Dictionary<string, string> queryParams, int requestCountInSecond, string path, int fromPage, int toPage, CancellationToken ct);
    }
    
    /// <summary>
    /// Class for working with multiple sending Http receipts and receiving data from their responses.
    /// </summary>
    internal class ManyRequestsSender : IManyRequestsSender
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
        /// Class for catching exceptions by http status code.
        /// </summary>
        readonly HttpInvalidCodeHandler httpInvalidCodeHandler;
        
        /// <summary>
        /// An Http multi send constructor that accepts custom parameters.
        /// </summary>
        /// <param name="httpClient">Class for sending HTTP requests and receiving HTTP responses from a resource specified by a URI.</param>
        /// <param name="serializer">Class that are used when serializing and deserializing JSON.</param>
        /// <param name="httpInvalidCodeHandler">Class for catching exceptions by http status code.</param>
        internal ManyRequestsSender(IHttpClient httpClient, ISerializer serializer, HttpInvalidCodeHandler httpInvalidCodeHandler)
        {
            this.httpClient = httpClient;
            this.serializer = serializer;
            this.httpInvalidCodeHandler = httpInvalidCodeHandler;
        }
        
        /// <summary>
        /// Asynchronous method for getting data from multiple requests.
        /// </summary>
        /// <param name="queryParams">Contains query parameter as a key, and its query value as a value.</param>
        /// <param name="requestCountInSecond">Requests per second limit.</param>
        /// <param name="path">Contains protocol, host and resource path.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection.</returns>
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
        
        /// <summary>
        /// Receiving responses to Internet requests as an asynchronous operation.
        /// </summary>
        /// <param name="requestUrls">Multiple Request URLs.</param>
        /// <param name="requestCountInSecond">Requests per second limit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of responses to an Internet requests as an asynchronous operation.</returns>
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
