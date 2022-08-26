using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
