using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.Utils
{
    internal static class HttpResponseMessageExtensions
    {
        internal async static Task<string> ReadAsStringContentOrThrowExceptionAsync(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
