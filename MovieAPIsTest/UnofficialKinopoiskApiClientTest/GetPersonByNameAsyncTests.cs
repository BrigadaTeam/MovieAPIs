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
    public class GetPersonByNameAsyncTests
    {
        UnofficialKinopoiskHttpInvalidCodeHandler httpInvalidCodeHandler;
        [OneTimeSetUp]
        public void Setup()
        {
            httpInvalidCodeHandler = new UnofficialKinopoiskHttpInvalidCodeHandler();
        }
        [Test]
         public void GetPersonByNameAsync_CorrectParam_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""kinopoiskId"":37859,""nameRu"":""Леонардо ДиКаприо""}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var personListAsync = client.GetPersonByNameAsync("Дикаприо").Result;
            Assert.IsTrue(personListAsync.Items[0].FilmId == 37859 && personListAsync.Items[0].NameRu == "Леонардо ДиКаприо");
        }

         [Test]
         public void GetPersonByNameAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPersonByNameAsync("Дикаприо"));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized].Message);
        }
        
        [Test]
        public void GetPersonByNameAsync_EmptyString_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?page=1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() =>  client.GetPersonByNameAsync(""));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest].Message);
        }

        [Test]
        public void GetPersonByNameAsync_NegativePage_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=-1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPersonByNameAsync("Дикаприо", page: -1));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest].Message);
        }
        
        [Test]
        public void GetImagesByIdAsync_BigPage_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=9999999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, It.IsAny<CancellationToken>()) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPersonByNameAsync("Дикаприо", page: 9999999));
            Assert.True(ex!.Message == httpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest].Message);
        }
    }
}

