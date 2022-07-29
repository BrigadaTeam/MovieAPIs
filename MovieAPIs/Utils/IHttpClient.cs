﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.Utils
{
    internal interface IHttpClient
    {
        Task<string> GetStringAsync(string requestUrl);
    }

    internal class InternalHttpClient : IHttpClient
    {
        HttpClient client;
        public InternalHttpClient(string apiKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        }
        public Task<string> GetStringAsync(string requestUrl)
        {
            return client.GetStringAsync(requestUrl);
        }
    }
}