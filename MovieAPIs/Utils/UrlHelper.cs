using System.Text;

namespace MovieAPIs.Utils
{
    static internal class UrlHelper
    {
        internal static string GetPath(params string[] pathSegments)
        {
            if (pathSegments.Length == 0 || pathSegments == null)
                return string.Empty;
            var path = new StringBuilder();
            for(int i = 0; i < pathSegments.Length; i++)
            {
                path.Append($"{pathSegments[i]}/");
            }
            return path.ToStringWithoutLastChar();
        }
        internal static string GetPathWithQuery(Dictionary<string, string> queryParams, params string[] pathSegments)
        {
            string path = GetPath(pathSegments);
            string query = GetQuery(queryParams);
            return $"{path}?{query}";
        }
        static string GetQuery(Dictionary<string, string> queryParams)
        {
            if (queryParams == null || queryParams.Count == 0)
                return string.Empty;
            var query = new StringBuilder();
            foreach (var queryParam in queryParams)
            {
                if(!String.IsNullOrEmpty(queryParam.Value))
                query.Append($"{queryParam.Key}={queryParam.Value}&");
            }
            return query.ToStringWithoutLastChar();
        }
    }
}
