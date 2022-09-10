using System.Collections.Generic;
using System.Net;
using System.Net.Http;


namespace MovieAPIs.Common.Http
{
    internal abstract class HttpInvalidCodeHandler
    {
        public abstract Dictionary<HttpStatusCode, HttpRequestException> Errors { get; }
        public void ThrowException(HttpStatusCode code)
        {
            if (Errors.ContainsKey(code))
                throw Errors[code];
            throw new HttpRequestException(code.ToString());
        }
    }
}
