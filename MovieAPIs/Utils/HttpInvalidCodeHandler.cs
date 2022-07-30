using System.Collections.Generic;
using System.Net;
using System.Net.Http;
namespace MovieAPIs.Utils
{
    internal static class HttpInvalidCodeHandler
    {
        internal static readonly Dictionary<int, string> Errors = new Dictionary<int, string>()
        {
            {401, "Empty or invalid token"},
            {402, "Request limit exceeded (either daily or total)"},
            {404, "Data not found"},
            {429, "Too many requests. General limit - 20 requests per second"}
        };
        internal static void ThrowException(HttpStatusCode code)
        {
            var exceptionMessage =
                Errors.ContainsKey((int)code) ? $"{Errors[(int)code]}" : code.ToString();
            
            throw new HttpRequestException(exceptionMessage);
        }
    }
}
