using System.Text.Json.Nodes;

namespace MovieAPIs.Utils
{
    internal class JsonConfiguration : IConfiguration
    {
        JsonNode root;
        internal JsonConfiguration(string pathToConfigFile)
        {
            using(var reader = new StreamReader(pathToConfigFile))
            {
                string json = reader.ReadToEnd();
                var jsonSerializerOptions = new JsonNodeOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                root = JsonNode.Parse(json, jsonSerializerOptions)!; 
            }
        }
        string IConfiguration.this[string path] { 
            get {
                string[] pathSegments = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var currentNode = root;
                foreach(var pathSegment in pathSegments)
                {
                    currentNode = currentNode[pathSegment]!;
                }
                return currentNode.ToString();
            } 
        }
    }
}
