using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAPIsTest.UnofficialKinopoiskApiClientTest
{
    internal static class MovieApiTestHelper
    {
        internal static async Task<HttpResponseMessage> GetHttpMessageAsync(TimeSpan time, HttpResponseMessage response, CancellationToken ct = default)
        {
            await Task.Delay(time, ct);
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

