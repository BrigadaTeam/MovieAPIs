using MovieAPIs.Utils;
using NUnit.Framework;
using System.Threading.Tasks;
using MovieAPIs;
using Moq;
using System.Net;
using System.Net.Http;

namespace MovieAPIsTest
{
    public class GetPersonByNameAsyncTests
    {
         [Test]

        public async Task GetPersonByNameAsync_CorrectParam_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"{""items"":[{""kinopoiskId"":37859,""nameRu"":""Леонардо ДиКаприо""}]}"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var personListAsync = client.GetPersonByNameAsync("Дикаприо").Result;
            Assert.IsTrue(personListAsync.Films[0].FilmId == 37859 && personListAsync.Films[0].NameRu == "Леонардо ДиКаприо");
        }

         [Test]
        
        public async Task GetPersonByNameAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPersonByNameAsync("Дикаприо"));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }
        
        [Test]
        
        public async Task GetPersonByNameAsync_EmptyString_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?page=1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() =>  client.GetPersonByNameAsync(""));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }

        [Test]
        
        public async Task GetPersonByNameAsync_NegativePage_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=-1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPersonByNameAsync("Дикаприо", page: -1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
        
        [Test]
        
        public async Task GetImagesByIdAsync_BigPage_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/persons?name=Дикаприо&page=9999999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetPersonByNameAsync("Дикаприо", page: 9999999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
    }
}

