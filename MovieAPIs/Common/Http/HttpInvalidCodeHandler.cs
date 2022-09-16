using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace MovieAPIs.Common.Http
{
    /// <summary>
    /// Abstract class for catching exceptions by http status code.
    /// </summary>
    internal abstract class HttpInvalidCodeHandler
    {
        /// <summary>
        /// Contains values of status codes defined for HTTP as key and a base class for exceptions thrown by the HttpClient and HttpMessageHandler classes as value.
        /// </summary>
        public abstract Dictionary<HttpStatusCode, HttpRequestException> Errors { get; }
        
        /// <summary>
        /// Check the http status code and throws the corresponding error.
        /// </summary>
        /// <param name="code">Contains the values of status codes defined for HTTP.</param>
        /// <exception cref="HttpRequestException">Thrown HttpRequestException.</exception>
        public void ThrowException(HttpStatusCode code)
        {
            if (Errors.ContainsKey(code))
                throw Errors[code];
            throw new HttpRequestException(code.ToString());
        }
    }
}
