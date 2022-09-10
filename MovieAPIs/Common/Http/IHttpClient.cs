using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIs.Common.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken ct = default);
    }
}
