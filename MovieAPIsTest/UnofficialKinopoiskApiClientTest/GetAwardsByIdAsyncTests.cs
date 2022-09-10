using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MovieAPIs.Common.Http;
using MovieAPIs.UnofficialKinopoiskApi;
using MovieAPIs.UnofficialKinopoiskApi.Http;
using NUnit.Framework;

namespace MovieAPIsTest.UnofficialKinopoiskApiClientTest
{
    public class GetAwardsByIdAsyncTests
    {
        UnofficialKinopoiskHttpInvalidCodeHandler httpInvalidCodeHandler;
        [OneTimeSetUp]
        public void Setup()
        {
            httpInvalidCodeHandler = new UnofficialKinopoiskHttpInvalidCodeHandler();
        }
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
            var awardsByIdAsync = client.GetAwardsByIdAsync(420923).Result;
            Assert.IsTrue(awardsByIdAsync.Items[0].Win == false && awardsByIdAsync.Items[0].Name == "Сатурн");
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
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(999999999));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest].Message);
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
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(999));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized].Message);
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
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(-1));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.NotFound].Message);
        }

        [TestCase(100, 50)]
        [TestCase(400, 200)]
        [TestCase(1000, 500)]
        public async Task CancellationTokenTests_CompareRunningTime_SuccessfulTaskAbort(int runningMilliseconds, int millisecondsDelay)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Accepted
            };
            
            var cSource = new CancellationTokenSource();

            IHttpClient httpClient = Mock.Of<IHttpClient>(x =>
                    x.GetAsync(It.IsAny<string>(), cSource.Token) == MovieApiTestHelper.GetHttpMessageAsync(TimeSpan.FromMilliseconds(runningMilliseconds), response, cSource.Token));
            UnofficialKinopoiskApiClient client = new UnofficialKinopoiskApiClient(httpClient);

            var timeTask1 = MovieApiTestHelper.GetWorkTime(async () =>
            {
                await client.GetAwardsByIdAsync(420923, cSource.Token);
            });

            httpClient = Mock.Of<IHttpClient>(x =>
                        x.GetAsync(It.IsAny<string>(), cSource.Token) == MovieApiTestHelper.GetHttpMessageAsync(TimeSpan.FromMilliseconds(runningMilliseconds), response, cSource.Token));
            client = new UnofficialKinopoiskApiClient(httpClient);

            var timeTask2 = MovieApiTestHelper.GetWorkTime(async () =>
            {
                    var task = client.GetAwardsByIdAsync(420925, cSource.Token);
                    cSource.CancelAfter(millisecondsDelay);
                    await task;
            });

            var time1 = await timeTask1;
            var time2 = await timeTask2;
            var cancelTime = time2 - TimeSpan.FromMilliseconds(millisecondsDelay);
            var maxCancelTime = TimeSpan.FromMilliseconds(150);
            Assert.That(time1, Is.GreaterThan(time2));
            Assert.That(cancelTime, Is.LessThan(maxCancelTime));
        }
    }
}

