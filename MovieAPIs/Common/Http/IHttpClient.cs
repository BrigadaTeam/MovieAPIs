using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Common.Http
{
    /// <summary>
    /// Interface for working with a HttpClient class.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUrl">Request URL.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task object that represents  a HTTP response message including the status code and data.</returns>
        Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken ct = default);
    }
}
