using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = "cfbe1246-e6eb-4ae8-bbd0-6fb17b9223b8";
            var client = new UnofficialKinopoiskApiClient(key);

            var answ = client.GetRelatedFilmsAsync();

            foreach (var item in answ.Result.Films)
            {
                Console.WriteLine(item.NameRu);
            }
        }
    }
}
