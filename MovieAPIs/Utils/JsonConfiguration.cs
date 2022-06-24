using System;
using System.IO;
using System.Text.Json;

namespace MovieAPIs.Utils
{
    internal class JsonConfiguration : IConfiguration
    {
        JsonDocument document;
        JsonSerializerOptions options;
        internal JsonConfiguration(string pathToConfigFile)
        {
            using(var reader = new StreamReader(pathToConfigFile))
            {
                string json = reader.ReadToEnd();
                document = JsonDocument.Parse(json);
                options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            }
        }
        string IConfiguration.this[string path] { 
            get {
                string[] pathSegments = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var currentNode = document.RootElement;
                foreach(var pathSegment in pathSegments)
                {
                    currentNode = currentNode.GetProperty(pathSegment);
                }
                return currentNode.ToString()!;
            } 
        }
    }
}
