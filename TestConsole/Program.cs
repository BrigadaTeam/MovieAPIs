using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = "API-KEY";
            var client = new UnofficialKinopoiskApiClient(key);

            var answ = client.GetTopFilmsAsync();

            foreach (var item in answ.Result.Films)
            {
                Console.WriteLine(item.NameRu);
            }           
        }
    }
}
