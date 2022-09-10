using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MovieAPIs.Common.Http;
using MovieAPIs.UnofficialKinopoiskApi;
using MovieAPIs.UnofficialKinopoiskApi.Models;
using NUnit.Framework;

namespace MovieAPIsTest.UnofficialKinopoiskApiClientTest
{
    public class GetImagesByIdAsyncTests
    {
        [Test]
        public void GetImagesByIdAsync_ExistingId_CorrectResult()
        {
            var imUrl =
                "https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/4e612862-882c-4427-acb9-da1969bc7ef4/orig";
            var prUrl =
                "https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/4e612862-882c-4427-acb9-da1969bc7ef4/300x";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""imageUrl"":""https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/4e612862-882c-4427-acb9-da1969bc7ef4/orig"",""previewUrl"":""https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/4e612862-882c-4427-acb9-da1969bc7ef4/300x""}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/665/images?page=1&type=STILL";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var imagesByIdAsync = client.GetImagesByIdAsync(665).Result;
            Assert.IsTrue(imagesByIdAsync.Films[0].ImageUrl == imUrl && imagesByIdAsync.Films[0].PreviewUrl == prUrl);
        }
        
        [Test]
        public void GetImagesByIdAsync_ExistingType_CorrectResult()
        {
            var imUrl =
                "https://avatars.mds.yandex.net/get-kinopoisk-image/1773646/2fcef5ef-32b8-4e5e-b11f-4d3b0f7a9f6a/orig";
            var prUrl =
                "https://avatars.mds.yandex.net/get-kinopoisk-image/1773646/2fcef5ef-32b8-4e5e-b11f-4d3b0f7a9f6a/300x";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""imageUrl"":""https://avatars.mds.yandex.net/get-kinopoisk-image/1773646/2fcef5ef-32b8-4e5e-b11f-4d3b0f7a9f6a/orig"",""previewUrl"":""https://avatars.mds.yandex.net/get-kinopoisk-image/1773646/2fcef5ef-32b8-4e5e-b11f-4d3b0f7a9f6a/300x""}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/665/images?page=1&type=POSTER";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var imagesByIdAsync = client.GetImagesByIdAsync(665, type: ImageType.POSTER).Result;
            Assert.IsTrue(imagesByIdAsync.Films[0].ImageUrl == imUrl && imagesByIdAsync.Films[0].PreviewUrl == prUrl);
        }
        
        [Test]
        public void GetImagesByIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/99999999/images?page=1&type=STILL";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetImagesByIdAsync(99999999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
        
        [Test]
        public void GetImagesByIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999/images?page=1&type=STILL";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => 
                client.GetImagesByIdAsync(999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }
        
        [Test]
        public void GetImagesByIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/-1/images?page=1&type=STILL";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetImagesByIdAsync(-1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
        
        [Test]
        public void GetImagesByIdAsync_UncorrectType_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999/images?page=1&type=999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => 
                client.GetImagesByIdAsync(999, type: (ImageType)999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
        
        [Test]
        public void GetImagesByIdAsync_NegativePage_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/665/images?page=-1&type=STILL";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => 
                client.GetImagesByIdAsync(665, page: -1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
        
        [Test]
        public void GetImagesByIdAsync_BigPage_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/665/images?page=100&type=STILL";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => 
                client.GetImagesByIdAsync(665, page: 100));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
    }
}

