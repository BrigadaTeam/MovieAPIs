using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Common.Http
{
    /// <summary>
    /// Sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Send a GET request to the specified Uri.
        /// </summary>
        /// <param name="requestUrl">Url on which the GET request is made.</param>
        /// <returns>HTTP response message including the status code and data.</returns>
        Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken ct = default); 
    }
}
