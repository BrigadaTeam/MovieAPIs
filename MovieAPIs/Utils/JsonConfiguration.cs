using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MovieAPIs.Utils
{
    internal class JsonConfiguration : IConfiguration
    {
        JsonDocument document;
        internal JsonConfiguration(string pathToConfigFile)
        {
            using(var reader = new StreamReader(pathToConfigFile))
            {
                string json = reader.ReadToEnd(); 
                document = JsonDocument.Parse(json);
            }
        }
        string IConfiguration.this[string path] { 
            get {
                string[] pathSegments = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var currentNode = document.RootElement;
                foreach(var pathSegment in pathSegments)
                {
                    currentNode = currentNode.EnumerateObject()
                        .FirstOrDefault(x => string.Compare(x.Name, pathSegment, StringComparison.OrdinalIgnoreCase) == 0)
                        .Value;
                }
                return currentNode.ToString()!;
            } 
        }
    }
}
