using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Threading;
using MovieAPIs.Common.Http;
using MovieAPIs.Common.Serialization;
using MovieAPIs.UnofficialKinopoiskApi.Models;

namespace MovieAPIsTest
{
    public class ManyRequestsHelperTests
    {
        ISerializer serializer;
        Dictionary<string, string> queryParams;

        [OneTimeSetUp]
        public void Setup()
        {
            serializer = new NewtonsoftJsonSerializer();
            queryParams = new Dictionary<string, string>
            {
                ["type"] = Tops.TOP_250_BEST_FILMS.ToString()
            };
        }
        
         [Test]
        public async Task QuickResponses()
        {
            var time = await Time(async () =>
            {
                IHttpClient httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None) == GetHttpMessageAsync(TimeSpan.FromMilliseconds(100)));
                var manyRequests = new ManyRequestsSender(httpClient, serializer);
                int expectedCount = 13;
                int requestCountInSecond = 5;
                var dataFromAllPages = manyRequests.GetDataAsync<Nomination>(queryParams, requestCountInSecond, "testUrl", 1, expectedCount, CancellationToken.None);
                int count = 0;
                await foreach(var dataFromPage in dataFromAllPages)
                {
                    Assert.AreEqual(dataFromPage.Name, "Сатурн");
                    count++;
                }
                Assert.AreEqual(expectedCount, count);
            });
            Assert.That(time, Is.GreaterThan(TimeSpan.FromSeconds(3))); // expectedCount(13) / requestCountInSecond(5) = min operating time(3)
            Assert.That(time, Is.LessThan(TimeSpan.FromSeconds(3.4)));
        }

        [Test]
        public async Task SlowResponses()
        {
            var time = await Time(async () =>
            {
                IHttpClient httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None) == GetHttpMessageAsync(TimeSpan.FromMilliseconds(1200)));
                var manyRequests = new ManyRequestsSender(httpClient, serializer);
                int expectedCount = 13;
                int requestCountInSecond = 5;
                var dataFromAllPages = manyRequests.GetDataAsync<Nomination>(queryParams, requestCountInSecond, "testUrl", 1, expectedCount, CancellationToken.None);
                int count = 0;
                await foreach (var dataFromPage in dataFromAllPages)
                {
                    Assert.AreEqual(dataFromPage.Name, "Сатурн");
                    count++;
                }
                Assert.AreEqual(expectedCount, count);
            });
            Assert.That(time, Is.GreaterThan(TimeSpan.FromSeconds(3))); // expectedCount(13) / requestCountInSecond(5) = min operating time(3)
            Assert.That(time, Is.LessThan(TimeSpan.FromSeconds(3.4)));
        }

        private async Task<HttpResponseMessage> GetHttpMessageAsync(TimeSpan time)
        {
            await Task.Delay(time);
            return await Task.FromResult(new HttpResponseMessage { 
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""name"":""Сатурн""}]}")
            });
        }
        private async Task<TimeSpan> Time(Func<Task> toTime)
        {
            var timer = Stopwatch.StartNew();
            await toTime();
            timer.Stop();
            return timer.Elapsed;
        }
    }
}
