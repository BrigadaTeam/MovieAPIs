using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs
{
    public abstract class MovieApiClientBase
    {
        protected async Task<T> GetResponseDataAsync<T>(string path, Dictionary<string, string>? queryParams = null)
        {
            string url = GetUrl(path, queryParams);
            HttpResponseMessage response = await GetResponse(url);
            ValidateResponse(response);
            string json = await response.Content.ReadAsStringAsync();
            return await Deserialize<T>(json);
        }

        protected abstract string GetUrl(string path, Dictionary<string, string>? queryParams);     
        protected abstract Task<HttpResponseMessage> GetResponse(string url);
        protected abstract void ValidateResponse(HttpResponseMessage response);
        protected abstract Task<T> Deserialize<T>(string json);
    }
}
