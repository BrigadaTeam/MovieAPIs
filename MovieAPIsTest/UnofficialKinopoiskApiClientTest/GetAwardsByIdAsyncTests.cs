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
    public class GetAwardsByIdAsyncTests
    {
        
        [Test]

        public async Task GetAwardsByIdAsync_ExistingId_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""name"":""Сатурн"", ""win"":false}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/420923/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var awardsByIdAsync = client.GetAwardsByIdAsync(420923).Result;
            Assert.IsTrue(awardsByIdAsync.Films[0].Win == false && awardsByIdAsync.Films[0].Name == "Сатурн");
        }

        [Test]
        
        public async Task GetAwardsByIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999999999/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(999999999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
        
        [Test]
        
        public async Task GetAwardsByIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/999/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }
        
        [Test]
        
        public async Task GetAwardsByIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/-1/awards";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetAwardsByIdAsync(-1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
    }
}

