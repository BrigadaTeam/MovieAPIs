using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = "Your api key";
            var client = new UnofficialKinopoiskApiClient(key);
        }
    }
}
