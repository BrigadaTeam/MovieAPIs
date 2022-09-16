using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MovieAPIs.Common.Http;

namespace MovieAPIs.UnofficialKinopoiskApi.Http
{
    /// <summary>
    /// Class for catching exceptions by http status code.
    /// </summary>
    internal class UnofficialKinopoiskHttpInvalidCodeHandler : HttpInvalidCodeHandler
    {
        /// <summary>
        /// Contains values of status codes defined for HTTP as key and a base class for exceptions thrown by the HttpClient and HttpMessageHandler classes as value.
        /// </summary>
        public override Dictionary<HttpStatusCode, HttpRequestException> Errors { get; } = new()
        {
            {HttpStatusCode.Unauthorized, new HttpRequestException("Empty or invalid token")},
            {HttpStatusCode.PaymentRequired, new HttpRequestException("Request limit exceeded (either daily or total)")},
            { HttpStatusCode.NotFound, new HttpRequestException("Data not found")},
            { HttpStatusCode.TooManyRequests, new HttpRequestException("Too many requests. General limit - 20 requests per second")},
            { HttpStatusCode.BadRequest, new HttpRequestException("Request entered incorrectly")}
        };
    }
}
