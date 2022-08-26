using System.Net;
using MovieAPIs.Utils;
using NUnit.Framework;
using System.Threading.Tasks;
using MovieAPIs;
using Moq;
using System.Net.Http;
using System.Threading;

namespace MovieAPIsTest
{
    public class CancellationTokenTest
    {
        private CancellationTokenSource _cSource;
        private IHttpClient _httpClient;
        private HttpResponseMessage _responseMessage;
        private UnofficialKinopoiskApiClient _client;

        [SetUp]

        public void SetUp()
        {
            _cSource = new CancellationTokenSource();
            _responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            };
            _httpClient = Mock.Of<IHttpClient>(x => x.GetAsync(It.IsAny<string>(), _cSource.Token) == Task.FromResult(_responseMessage));
            _client = new UnofficialKinopoiskApiClient(_httpClient);
        }
        
        [Test]

        public async Task Cancel()
        {
            var flag = false;
            
            try
            {
                _cSource.Cancel();
                var result = await _client.GetFilmsByFiltersAsync(_cSource.Token);
            }
            catch (TaskCanceledException) when (_cSource.Token.IsCancellationRequested)
            {
                flag = true;
            }

            Assert.IsTrue(flag);
        }
        
        [Test]

        public async Task NoCancel()
        {
            var flag = false;
            
            try
            {
                var result = await _client.GetFilmsByFiltersAsync(_cSource.Token);
            }
            catch (TaskCanceledException) when (_cSource.Token.IsCancellationRequested)
            {
                flag = true;
            }

            Assert.IsTrue(!flag);
        }
    }
}

