using MovieAPIs.Common.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Common.Http
{
    internal static class HttpResponseMessageExtensions
    {
        internal static async Task<string> ReadAsStringContentOrThrowExceptionAsync(this HttpResponseMessage response, CancellationToken ct)
        {
            if (!response.IsSuccessStatusCode)
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            return await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        }
    }
}
