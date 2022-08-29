﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MovieAPIs;
using MovieAPIs.Models;
using MovieAPIs.Utils;
using NUnit.Framework;

namespace MovieAPIsTest.UnofficialKinopoiskApiClientTest
{
    public class GetPremieresListAsyncTests
    {
        [Test]
        public void GetPremieresListAsync_CorrectParam_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""kinopoiskId"":1043758,""nameRu"":""Паразиты""}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=2019&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var premieresListAsync = client.GetPremieresListAsync(2019, Months.JULY, It.IsAny<CancellationToken>()).Result;
            Assert.IsTrue(premieresListAsync.Films[0].KinopoiskId == 1043758 && premieresListAsync.Films[0].NameRu == "Паразиты");
        }

        [Test]
        public void GetPremieresListAsync_BigYear_EmptyResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""items"":[]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=5000&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var premieresListAsync = client.GetPremieresListAsync(5000, Months.JULY, It.IsAny<CancellationToken>()).Result;
            Assert.IsTrue(premieresListAsync.Films.Length == 0);
        }
        
        [Test]
        public void GetPremieresListAsync_NegativeYear_EmptyResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""items"":[]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=-5000&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var premieresListAsync = client.GetPremieresListAsync(-5000, Months.JULY, It.IsAny<CancellationToken>()).Result;
            Assert.IsTrue(premieresListAsync.Films.Length == 0);
        }
        
        [Test]
        public void GetPremieresListAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=2020&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPremieresListAsync(2020, Months.JULY, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }

        [Test]
        public void GetPremieresListAsync_InvalidMonth_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=2020&month=959";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPremieresListAsync(2020, (Months)959, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
    }
}

