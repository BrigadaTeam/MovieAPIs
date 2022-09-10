using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MovieAPIs.Common.Http;

namespace MovieAPIs.UnofficialKinopoiskApi.Http
{
    internal class UnofficialKinopoiskHttpInvalidCodeHandler : HttpInvalidCodeHandler
    {
        readonly Dictionary<HttpStatusCode, HttpRequestException>  errors = new()
        {
            {HttpStatusCode.Unauthorized, new HttpRequestException("Empty or invalid token")},
            {HttpStatusCode.PaymentRequired, new HttpRequestException("Request limit exceeded (either daily or total)")},
            { HttpStatusCode.NotFound, new HttpRequestException("Data not found")},
            { HttpStatusCode.TooManyRequests, new HttpRequestException("Too many requests. General limit - 20 requests per second")},
            { HttpStatusCode.BadRequest, new HttpRequestException("Request entered incorrectly")}
        };

        public override Dictionary<HttpStatusCode, HttpRequestException> Errors => errors;
    }
}
