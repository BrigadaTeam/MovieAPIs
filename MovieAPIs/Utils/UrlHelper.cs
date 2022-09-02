using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace MovieAPIs.Utils
{
    internal static class UrlHelper
    {
        internal static string GetUrl(string path, Dictionary<string, string>? queryParams = null)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path must not be empty or null", nameof(path));
            string query = GetQuery(queryParams);
            if (string.IsNullOrEmpty(query))
                return path;
            return $"{path}?{query}";
        }
        internal static string GetQuery(Dictionary<string, string>? queryParams)
        {
            if (queryParams == null || queryParams.Count == 0)
                return string.Empty;
            var query = new StringBuilder();
            foreach (var queryParam in queryParams)
            {
                if(!string.IsNullOrEmpty(queryParam.Value))
                    query.Append($"{queryParam.Key}={queryParam.Value}&");
            }
            return query.ToString().TrimEnd('&');
        }

        internal static string[] GetUrls(Dictionary<string, string> queryParams, string path, int fromPage, int toPage)
        {
            var urls = new LinkedList<string>();

            for (int page = fromPage; page <= toPage; page++)
            {
                queryParams["page"] = page.ToString();
                string url = GetUrl(path, queryParams);
                urls.AddLast(url);
            }

            queryParams.Remove("page");
            return urls.ToArray();
        }
    }
}
