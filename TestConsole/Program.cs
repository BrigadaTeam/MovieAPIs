using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = "ApiKey";
            var client = new UnofficialKinopoiskApiClient(key);

            var answ = client.GetFullListTopFilmsAsync();

            Console.WriteLine(answ.Result.Films.Length);

        }
    }
}
