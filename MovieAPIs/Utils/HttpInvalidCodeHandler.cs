using System.Collections.Generic;
using System.Net;
using System.Net.Http;
namespace MovieAPIs.Utils
{
    internal static class HttpInvalidCodeHandler
    {
        internal static readonly Dictionary<HttpStatusCode, HttpRequestException> Errors = 
            new Dictionary<HttpStatusCode, HttpRequestException>()
        {
            {HttpStatusCode.Unauthorized, new HttpRequestException("Empty or invalid token")},
            {HttpStatusCode.PaymentRequired, new HttpRequestException("Request limit exceeded (either daily or total)")},
            {HttpStatusCode.NotFound, new HttpRequestException("Data not found")},
            {HttpStatusCode.TooManyRequests, new HttpRequestException("Too many requests. General limit - 20 requests per second")},
            {HttpStatusCode.BadRequest, new HttpRequestException("Invalid id entered")}
        };
        internal static void ThrowException(HttpStatusCode code)
        {
            if (Errors.ContainsKey(code))
                throw Errors[code];
            throw new HttpRequestException(code.ToString());
        }
    }
}
