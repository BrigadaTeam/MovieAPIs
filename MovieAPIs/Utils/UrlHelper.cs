using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieAPIs.Utils
{
    static internal class UrlHelper
    {
        internal static string GetPath(params string[] pathSegments)
        {
            if (pathSegments == null || pathSegments.Length == 0)
                return string.Empty;
            var path = new StringBuilder();
            for(int i = 0; i < pathSegments.Length; i++)
            {
                path.Append($"{pathSegments[i]}/");
            }
            return path.ToString().TrimEnd('/');
        }
        internal static string GetPathWithQuery(Dictionary<string, string> queryParams, params string[] pathSegments)
        {
            string path = GetPath(pathSegments);
            string query = GetQuery(queryParams);
            if(path == string.Empty || query == string.Empty)
            {
                return path == string.Empty ? query : path;
            }
            return $"{path}?{query}";
        }
        internal static string GetQuery(Dictionary<string, string> queryParams)
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

        internal static string[] GetUrls(Dictionary<string, string> queryParams, int pageCount, params string[] pathSegments)
        {
            string path = GetPath(pathSegments);
            var urls = new LinkedList<string>();

            for(int page = 1; page <= pageCount; page++)
            {
                queryParams["page"] = page.ToString();
                string query = GetQuery(queryParams);
                urls.AddLast($"{path}?{query}");
            }

            queryParams.Remove("page");

            return urls.ToArray();
        }
    }
}
