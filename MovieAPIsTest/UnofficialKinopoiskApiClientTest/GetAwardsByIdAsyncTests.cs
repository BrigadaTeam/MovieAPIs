using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MovieAPIs;
using MovieAPIs.Utils;
using NUnit.Framework;

namespace MovieAPIsTest.UnofficialKinopoiskApiClientTest
{
    public class GetAwardsByIdAsyncTests
    {
        [Test]
        public void GetAwardsByIdAsync_ExistingId_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""name"":""Сатурн"", ""win"":false}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/420923/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var awardsByIdAsync = client.GetAwardsByIdAsync(420923, It.IsAny<CancellationToken>()).Result;
            Assert.IsTrue(awardsByIdAsync.Films[0].Win == false && awardsByIdAsync.Films[0].Name == "Сатурн");
        }
        
        [Test]
        public void GetAwardsByIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999999999/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(999999999, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
        
        [Test]
        public void GetAwardsByIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(999, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }
        
        [Test]
        public void GetAwardsByIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/-1/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(-1, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
        
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public async Task CancellationTokenTests_CompareRunningTime_SuccessfulTaskAbort(int runningTime)
        {
            var cSource = new CancellationTokenSource();
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Accepted
            };
            var httpClient = Mock.Of<IHttpClient>(x =>
                x.GetAsync(It.IsAny<string>(), cSource.Token) == MovieApiTestHelper.GetHttpMessageAsync(TimeSpan.FromMilliseconds(runningTime), response));

            var client = new UnofficialKinopoiskApiClient(httpClient);

            var time1 = await MovieApiTestHelper.GetWorkTime(async () =>
            {
                await client.GetAwardsByIdAsync(420923, cSource.Token);
            });
            
            var time2 = await MovieApiTestHelper.GetWorkTime(async () =>
            {
                await client.GetAwardsByIdAsync(420923, cSource.Token);
                cSource.Cancel();
            });
            
            Assert.IsTrue(time1 > time2);
            Assert.IsTrue(time2 < TimeSpan.FromMilliseconds(1));
        }
    }
}

