using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.Utils
{
    internal static class RequestHelper
    {
        private static int delay = 200; // to avoid error 429
        public static async Task<List<string>> GetListOfResponsesAsync(HttpClient client, string[] urlPathsWithQuery)
        {
            var tasks = new List<Task<string>>();
            var requestResponseBody = new List<string>();

            foreach (var urlPathWithQuery in urlPathsWithQuery)
            {
                tasks.Add(client.GetStringAsync(urlPathWithQuery));
                Thread.Sleep(delay);
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                requestResponseBody.Add(await task);
            }

            return requestResponseBody;
        }
    }
}
