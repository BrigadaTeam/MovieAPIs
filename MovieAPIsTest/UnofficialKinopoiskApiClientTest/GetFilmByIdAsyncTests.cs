using MovieAPIs.Utils;
using NUnit.Framework;
using System.Threading.Tasks;
using MovieAPIs;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace MovieAPIsTest
{
    public class GetFilmByIdAsyncTests
    {
        [Test]

        public async Task GetFilmByIdAsync_ExistingId_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""kinopoiskId"":301,""nameRu"":""Матрица""}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/301";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var film = await client.GetFilmByIdAsync(301);
 
            Assert.IsTrue(film.KinopoiskId == 301 && film.NameRu == "Матрица");
        }
        
        [Test]
        
        public async Task GetFilmByIdAsync_NonExistingId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
        
        [Test]
        
        public async Task GetFilmByIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999999999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(999999999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
        
        [Test]
        
        public async Task GetFilmByIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }
        
        [Test]
        
        public async Task GetFilmByIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/-1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetFilmByIdAsync(-1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
    }
}

