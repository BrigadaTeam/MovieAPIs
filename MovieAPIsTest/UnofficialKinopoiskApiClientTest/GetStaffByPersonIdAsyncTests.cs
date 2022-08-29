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
    public class GetStaffByPersonIdAsyncTests
    {
        [Test]
        public void GetStaffByPersonIdAsync_ExistingId_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""personId"":37859,""nameRu"":""Леонардо ДиКаприо""}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff/37859";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var staffByPersonIdAsync = client.GetStaffByPersonIdAsync(37859, It.IsAny<CancellationToken>()).Result;
            Assert.IsTrue(staffByPersonIdAsync.PersonId == 37859 && staffByPersonIdAsync.NameRu == "Леонардо ДиКаприо");
        }

        [Test]
        public void GetStaffByPersonIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff/99999999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetStaffByPersonIdAsync(99999999, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
   
        [Test]
        public void GetStaffByPersonIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff/37859";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetStaffByPersonIdAsync(37859, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }

        [Test]
        public void GetStaffByPersonIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff/-1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetStaffByPersonIdAsync(-1, It.IsAny<CancellationToken>()));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
    }
}

