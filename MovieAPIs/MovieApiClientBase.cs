﻿using MovieAPIs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieAPIs
{
    public abstract class MovieApiClientBase
    {
        readonly IHttpClient httpClient;
        readonly ISerializer serializer;
        internal readonly IManyRequestsHelper manyRequestsHelper;
        public MovieApiClientBase(IHttpClient httpClient, ISerializer serializer)
        {
            this.serializer = serializer;
            this.httpClient = httpClient;
            manyRequestsHelper = new ManyRequestsHelper(httpClient, serializer);
        }
        protected async Task<T> GetResponseDataAsync<T>(string path, Dictionary<string, string>? queryParams = null)
        {
            string url = UrlHelper.GetUrl(path, queryParams);
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                HttpInvalidCodeHandler.ThrowException(response.StatusCode);
            string json = await response.Content.ReadAsStringAsync();
            return serializer.Deserialize<T>(json);
        }
    }
}
