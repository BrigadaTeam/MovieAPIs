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
    public class GetTrailersAndTeasersByIdAsyncTests
    {
        [Test]
        public void GetTrailersAndTeasersByIdAsync_ExistingId_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""items"":[{""name"":""Тизер"",""site"":""KINOPOISK_WIDGET""}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/5656/videos";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var trailersAndTeasersByIdAsync = client.GetTrailersAndTeasersByIdAsync(5656).Result;
            Assert.IsTrue(trailersAndTeasersByIdAsync.Films[0].Name == "Тизер" && trailersAndTeasersByIdAsync.Films[0].Site == "KINOPOISK_WIDGET");
        }

        [Test]
        public void GetTrailersAndTeasersByIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/9999999/videos";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var exception = Assert.ThrowsAsync<HttpRequestException>(() =>  
                client.GetTrailersAndTeasersByIdAsync(9999999));
            Assert.IsTrue(exception! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }

        [Test]
        public void GetTrailersAndTeasersByIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999/videos";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetTrailersAndTeasersByIdAsync(999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }

        [Test]
        public void GetTrailersAndTeasersByIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/-1/videos";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetTrailersAndTeasersByIdAsync(-1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
    }
}

