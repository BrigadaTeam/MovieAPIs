using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = "Some api key";
            var client = new UnofficialKinopoiskApiClient(key);

            var answ = client.GetFilmsByFiltersAsync(2, 1);

            foreach (var genre in answ.Result.Items)
            {
                Console.WriteLine(genre.NameRu);
            }
        }
    }
}
