﻿using MovieAPIs.Utils;
using NUnit.Framework;
using System.Threading.Tasks;
using MovieAPIs;
using Moq;
using System.Net;
using System.Net.Http;
using MovieAPIs.Models;

namespace MovieAPIsTest
{
    public class GetPremieresListAsyncTests
    {
        [Test]

        public async Task GetPremieresListAsync_CorrectParam_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""kinopoiskId"":1043758,""nameRu"":""Паразиты""}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=2019&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var premieresListAsync = client.GetPremieresListAsync(2019, Months.JULY).Result;
            Assert.IsTrue(premieresListAsync.Films[0].KinopoiskId == 1043758 && premieresListAsync.Films[0].NameRu == "Паразиты");
        }

        [Test]
        
        public async Task GetPremieresListAsync_BigYear_EmptyResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""items"":[]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=5000&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var premieresListAsync = client.GetPremieresListAsync(5000, Months.JULY).Result;
            Assert.IsTrue(premieresListAsync.Films.Length == 0);
        }
        
        [Test]
        
        public async Task GetPremieresListAsync_NegativeYear_EmptyResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""items"":[]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=-5000&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var premieresListAsync = client.GetPremieresListAsync(-5000, Months.JULY).Result;
            Assert.IsTrue(premieresListAsync.Films.Length == 0);
        }
        
        [Test]
        
        public async Task GetPremieresListAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=2020&month=JULY";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPremieresListAsync(2020, Months.JULY));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }

        [Test]
        
        public async Task GetPremieresListAsync_InvalidMonth_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/premieres?year=2020&month=959";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPremieresListAsync(2020, (Months)959));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
    }
}
