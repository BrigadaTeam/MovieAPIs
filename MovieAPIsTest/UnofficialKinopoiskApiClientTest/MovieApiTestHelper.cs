﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieAPIsTest.UnofficialKinopoiskApiClientTest
{
    internal static class MovieApiTestHelper
    {
        internal static async Task<HttpResponseMessage> GetHttpMessageAsync(TimeSpan time, HttpResponseMessage response)
        {
            await Task.Delay(time);
            return await Task.FromResult(response);
        }
        
        internal static async Task<TimeSpan> GetWorkTime(Func<Task> toTime)
        {
            var timer = Stopwatch.StartNew();
            await toTime();
            timer.Stop();
            return timer.Elapsed;
        }
    }
}

