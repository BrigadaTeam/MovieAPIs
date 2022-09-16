using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace MovieAPIs.Common.Helper
{
    /// <summary>
    /// Class for constructing URLs.
    /// </summary>
    internal static class UrlHelper
    {
        /// <summary>
        /// Method returning final URL with parameters.
        /// </summary>
        /// <param name="path">Contains protocol, host and resource path.</param>
        /// <param name="queryParams">Contains query parameter as a key, and its query value as a value.</param>
        /// <returns>URL path with parameters.</returns>
        /// <exception cref="ArgumentException">Empty or null path.</exception>
        internal static string GetUrl(string path, Dictionary<string, string>? queryParams = null)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path must not be empty or null", nameof(path));
            string query = GetQuery(queryParams);
            if (string.IsNullOrEmpty(query))
                return path;
            return $"{path}?{query}";
        }
        
        /// <summary>
        /// Method concatenates URL parameters into one string from separate parameters.
        /// </summary>
        /// <param name="queryParams">Contains query parameter as a key, and its query value as a value.</param>
        /// <returns>String containing all query parameters.</returns>
        internal static string GetQuery(Dictionary<string, string>? queryParams)
        {
            if (queryParams == null || queryParams.Count == 0)
                return string.Empty;
            var query = new StringBuilder();
            foreach (var queryParam in queryParams)
            {
                if (!string.IsNullOrEmpty(queryParam.Value))
                    query.Append($"{queryParam.Key}={queryParam.Value}&");
            }
            return query.ToString().TrimEnd('&');
        }
        
        /// <summary>
        /// Method returning end URLs with parameters with pagination.
        /// </summary>
        /// <param name="queryParams">Contains query parameter as a key, and its query value as a value.</param>
        /// <param name="path">Contains protocol, host and resource path.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End Page.</param>
        /// <returns>Array of URLs with parameters.</returns>
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
