using MovieAPIs.Common.Http;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MovieAPIsTest.Utils;
using MovieAPIs.Common.Responses;

namespace MovieAPIsTest.CommonTest
{
    public class MovieApiClientBaseTest
    {
        readonly HttpInvalidCodeHandlerTest httpInvalidCodeHandler = new();

        readonly Dictionary<string, string> queryParams = new()
        {
            ["name"] = "test",
            ["age"] = "18"
        };

        const string PATH = "TestPath";
        const string URL = $"{PATH}?name=test&age=18";

        [Test]
        public async Task GetResponseDataTestAsync_AcceptResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""result"": ""accept""}"),
            };

            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(URL, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new MovieApiClientTest(httpClient, httpInvalidCodeHandler);
            var data = await client.GetResponseDataAsync<DataTest>(PATH, CancellationToken.None, queryParams);
            Assert.AreEqual("accept", data.Result);
        }
        [Test]
        public async Task GetResponseDataTestAsync_InvalidResult()
        {
            var code = HttpStatusCode.NotFound;
            var response = new HttpResponseMessage
            {
                StatusCode = code
            };
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(URL, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new MovieApiClientTest(httpClient, httpInvalidCodeHandler);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetResponseDataAsync<DataTest>(PATH, CancellationToken.None, queryParams));
            Assert.AreEqual(httpInvalidCodeHandler.Errors[code].Message, ex.Message);
        }

        [TestCase(5, 1, 7, 1, 10, 21)]
        public async Task GetResponsesDataFromPageRangeAsync_AcceptResult(int requestCountInSecond, int fromPage, int toPage, int firstPage, int pagesCount, int expectedDataCount)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{ ""pagesCount"": ""10"", ""items"": [{""result"": ""accept""}, {""result"": ""accept""}, {""result"": ""accept""}]}"),
            };

            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new MovieApiClientTest(httpClient, httpInvalidCodeHandler);
            int dataCount = 0;
            await foreach (var data in client.GetResponsesDataFromPageRangeAsync<DataTest>(URL, queryParams, requestCountInSecond, fromPage, toPage, firstPage, pagesCount, CancellationToken.None))
            {
                Assert.AreEqual("accept", data.Result);
                dataCount++;
            }
            Assert.AreEqual(expectedDataCount, dataCount);

        }

        [TestCase(5, 1, 7, 1, 10)]
        public async Task GetResponsesDataFromPageRangeAsync_InvalidResult(int requestCountInSecond, int fromPage, int toPage, int firstPage, int pagesCount)
        {
            var code = HttpStatusCode.NotFound;
            var response = new HttpResponseMessage
            {
                StatusCode = code
            };

            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new MovieApiClientTest(httpClient, httpInvalidCodeHandler);
            var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await foreach (var data in client.GetResponsesDataFromPageRangeAsync<DataTest>(URL, queryParams, requestCountInSecond, fromPage, toPage, firstPage, pagesCount, CancellationToken.None))
                {
                }
            });
            Assert.AreEqual(httpInvalidCodeHandler.Errors[code].Message, ex.Message);

        }
    }
}
