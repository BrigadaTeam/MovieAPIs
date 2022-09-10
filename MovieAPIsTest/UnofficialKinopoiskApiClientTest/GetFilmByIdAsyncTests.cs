﻿using System.Net;
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
    public class GetFilmByIdAsyncTests
    {
        UnofficialKinopoiskHttpInvalidCodeHandler httpInvalidCodeHandler;
        [OneTimeSetUp]
        public void Setup()
        {
            httpInvalidCodeHandler = new UnofficialKinopoiskHttpInvalidCodeHandler();
        }
        [Test]
        public async Task GetFilmByIdAsync_ExistingId_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""kinopoiskId"":301,""nameRu"":""Матрица""}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/301";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var film = await client.GetFilmByIdAsync(301);
 
            Assert.IsTrue(film.KinopoiskId == 301 && film.NameRu == "Матрица");
        }
        
        [Test]
        public void GetFilmByIdAsync_NonExistingId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(1, It.IsAny<CancellationToken>()));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.NotFound].Message);
        }
        
        [Test]
        public void GetFilmByIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999999999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(999999999, It.IsAny<CancellationToken>()));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest].Message);
        }
        
        [Test]
        public void GetFilmByIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(999, It.IsAny<CancellationToken>()));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized].Message);
        }
        
        [Test]
        public void GetFilmByIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/-1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(-1, It.IsAny<CancellationToken>()));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.NotFound].Message);
        }
    }
}

