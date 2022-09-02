using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MovieAPIs.Utils;
using System.Net.Http;
using System.Net;
using System.Threading;
using MovieAPIs.Models;
using MovieAPIsTest.UnofficialKinopoiskApiClientTest;

namespace MovieAPIsTest
{
    public class ManyRequestsHelperTests
    {
        ISerializer serializer;
        Dictionary<string, string> queryParams;
        HttpResponseMessage response;

        [OneTimeSetUp]
        public void Setup()
        {
            serializer = new NewtonsoftJsonSerializer();
            queryParams = new Dictionary<string, string>
            {
                ["type"] = Tops.TOP_250_BEST_FILMS.ToString()
            };
            response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Accepted
            };
        }
        
         [Test]
        public async Task QuickResponses()
        {
            var time = await Time(async () =>
            {
                IHttpClient httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None) == GetHttpMessageAsync(TimeSpan.FromMilliseconds(100)));
                var manyRequests = new ManyRequestsHelper(httpClient, serializer);
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
            Assert.That(time, Is.LessThan(TimeSpan.FromSeconds(3.2)));
        }

        [Test]
        public async Task SlowResponses()
        {
            var time = await Time(async () =>
            {
                IHttpClient httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None) == GetHttpMessageAsync(TimeSpan.FromMilliseconds(1200)));
                var manyRequests = new ManyRequestsHelper(httpClient, serializer);
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
        
        /*[Test]
        public async Task QuickResponses()
        {
            var time = await MovieApiTestHelper.GetWorkTime(async () =>
            {
                IHttpClient httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()) == MovieApiTestHelper.GetHttpMessageAsync(TimeSpan.FromMilliseconds(100), response));
                var manyRequests = new ManyRequestsHelper(httpClient, serializer);
                int expectedCount = 13;
                int requestCountInSecond = 5;
                var dataFromAllPages = manyRequests.GetDataAsync<Nomination>(queryParams, requestCountInSecond, "testUrl", 1, expectedCount, It.IsAny<CancellationToken>());
                int count = 0;
                await foreach(var dataFromPage in dataFromAllPages)
                {
                    Assert.AreEqual(dataFromPage.Name, "Сатурн");
                    count++;
                }
                Assert.AreEqual(expectedCount, count);
            });
            Assert.That(time, Is.GreaterThan(TimeSpan.FromSeconds(3))); // expectedCount(13) / requestCountInSecond(5) = min operating time(3)
            Assert.That(time, Is.LessThan(TimeSpan.FromSeconds(3.2)));
        }

        [Test]
        public async Task SlowResponses()
        {
            var time = await MovieApiTestHelper.GetWorkTime(async () =>
            {
                IHttpClient httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(),It.IsAny<CancellationToken>()) == MovieApiTestHelper.GetHttpMessageAsync(TimeSpan.FromMilliseconds(1200), response));
                var manyRequests = new ManyRequestsHelper(httpClient, serializer);
                int expectedCount = 13;
                int requestCountInSecond = 5;
                var dataFromAllPages = manyRequests.GetDataAsync<Nomination>(queryParams, requestCountInSecond, "testUrl", 1, expectedCount, It.IsAny<CancellationToken>());
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
        }*/
    }
}
