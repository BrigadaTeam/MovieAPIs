using MovieAPIs;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPIs.Utils;

namespace MovieAPIsTest
{
    public class UnofficialKinopoiskApiClientTests
    {
        [Test]
        public async Task GetFilmByIdTestAsync()
        {
            var response = @"{""kinopoiskId"":301,""nameRu"":""Матрица""}";
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/301";
            var httpClient = Mock.Of<IHttpClient>(x => x.GetStringAsync(url) == Task.FromResult(response));
            var client = new UnofficialKinopoiskApiClient(httpClient);
            var film = await client.GetFilmByIdAsync(301);
            Assert.IsTrue(film.KinopoiskId == 301 && film.NameRu == "Матрица");
        }
    }
}
