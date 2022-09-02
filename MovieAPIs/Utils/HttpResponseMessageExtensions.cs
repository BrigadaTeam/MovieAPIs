using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Utils
{
    internal static class HttpResponseMessageExtensions
    {
        internal static Task<string> ReadAsStringContentOrThrowExceptionAsync(this HttpResponseMessage response, CancellationToken ct)
        {
            if (!response.IsSuccessStatusCode)
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            return response.Content.ReadAsStringAsync(ct);
        }
    }
}
