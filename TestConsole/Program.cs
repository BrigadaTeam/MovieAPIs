using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = "Api-key";
            var client = new UnofficialKinopoiskApiClient(key);

            var answ = client.GetGenresAndCountriesAsync();

            foreach (var genre in answ.Result.Genres)
            {
                Console.WriteLine(genre.Name);
            }

            /*var answ = client.GetGenresAndCountriesIdAsync();

            foreach (var genre in answ.Result.Genres)
            {
                Console.WriteLine(genre.Name);
            }

            foreach (var genre in answ.Result.Countries)
            {
                Console.WriteLine(genre.Name);
            }*/
        }
    }
}
