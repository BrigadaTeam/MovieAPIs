using System;
using System.Threading.Tasks;
using MovieAPIs;

namespace TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new UnofficialKinopoiskApi("cfbe1246-e6eb-4ae8-bbd0-6fb17b9223b8");
            Film film = await client.GetFilmByIdAsync(301);
        }
    }
}
