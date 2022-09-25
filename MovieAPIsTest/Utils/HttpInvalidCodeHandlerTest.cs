using MovieAPIs.Common.Http;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace MovieAPIsTest.Utils
{
    internal class HttpInvalidCodeHandlerTest : HttpInvalidCodeHandler
    {
        public override Dictionary<HttpStatusCode, HttpRequestException> Errors { get; } = new()
        {
            {HttpStatusCode.Unauthorized, new HttpRequestException("Unauthorized")},
            {HttpStatusCode.PaymentRequired, new HttpRequestException("PaymentRequired")},
            {HttpStatusCode.NotFound, new HttpRequestException("NotFound")},
            {HttpStatusCode.TooManyRequests, new HttpRequestException("TooManyRequests")},
            {HttpStatusCode.BadRequest, new HttpRequestException("BadRequest")}
        };
    }
}
