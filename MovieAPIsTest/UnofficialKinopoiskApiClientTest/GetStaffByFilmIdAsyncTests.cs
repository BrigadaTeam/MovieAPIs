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
    public class GetStaffByFilmIdAsyncTests
    {
        [Test]

        public async Task GetStaffByFilmIdAsync_ExistingId_CorrectResult()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(@"[{""staffId"":42125, ""nameRu"":""Тодд Хейнс""}]"),
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff?filmId=665";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var staffByFilmIdAsync = client.GetStaffByFilmIdAsync(665).Result;
            Assert.IsTrue(staffByFilmIdAsync[0].StaffId == 42125 && staffByFilmIdAsync[0].NameRu == "Тодд Хейнс");
        }

        [Test]
        
        public async Task GetStaffByFilmIdAsync_LongId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff?filmId=99999999";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetStaffByFilmIdAsync(99999999));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.BadRequest]);
        }
 
        [Test]
        
        public async Task GetStaffByFilmIdAsync_EmptyOrWrongToken_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff?filmId=665";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetStaffByFilmIdAsync(665));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.Unauthorized]);
        }
   
        [Test]
        
        public async Task GetStaffByFilmIdAsync_NegativeId_Exception()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var url = "https://kinopoiskapiunofficial.tech/api/v1/staff?filmId=-1";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(url, CancellationToken.None) == Task.FromResult<HttpResponseMessage>(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => client.GetStaffByFilmIdAsync(-1));
            Assert.True(ex! == HttpInvalidCodeHandler.Errors[HttpStatusCode.NotFound]);
        }
    }
}

