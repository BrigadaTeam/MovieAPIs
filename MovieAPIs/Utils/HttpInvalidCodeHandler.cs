using System;
using System.Net;
using System.Net.Http;

namespace MovieAPIs.Utils
{
    internal static class HttpInvalidCodeHandler
    {
        internal static void ThrowException(HttpStatusCode code)
        {
            throw (int)code switch
            {
                401 => new HttpRequestException($"Empty or invalid token - {code}"),
                402 => new HttpRequestException($"Request limit exceeded (either daily or total) - {code}"),
                404 => new HttpRequestException($"Data not found - {code}"),
                429 => new HttpRequestException($"Too many requests. General limit - 20 requests per second - {code}"),
                _ => new Exception($"An error occurred with code {code}")
            };
        }
    }
}
