using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = "";
            var client = new UnofficialKinopoiskApiClient(key);
            var film = await client.GetFilmsByKeywordAsync("Матрица");
        }
    }
}
