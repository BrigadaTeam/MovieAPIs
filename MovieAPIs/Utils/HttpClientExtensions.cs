using System.Text;

namespace MovieAPIs.Utils
{
    static internal class HttpClientExtensions
    {
        internal static async Task<Stream> GetStreamWithQueryAsync(this HttpClient client, string url, Dictionary<string, string> parameters)
        {
            if(parameters == null || parameters.Count == 0)
            {
                throw new ArgumentException();
            }
            var builder = new UriBuilder(url);
            var query = new StringBuilder();
            foreach(var param in parameters)
            {
                query.Append($"{param.Key}={param.Value}&");
            }
            builder.Query = query.Remove(query.Length - 1, 1).ToString();

            return await client.GetStreamAsync(builder.Uri);
        }
    }
}
