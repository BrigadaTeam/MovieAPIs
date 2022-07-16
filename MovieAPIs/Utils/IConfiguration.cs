using System;
using System.IO;
using System.Text.Json.Nodes;

namespace MovieAPIs.Utils
{
    internal interface IConfiguration
    {
        internal string this[string path] { get; }
    }

    internal class InvalidPathException : Exception
    {
        public InvalidPathException(string message) : base(message) { }
        public InvalidPathException(string message, Exception innerException) : base(message, innerException) { }
    }

    internal class JsonConfiguration : IConfiguration
    {
        readonly JsonNode root;
        internal JsonConfiguration(string pathToConfigFile)
        {
            using (var reader = new StreamReader(pathToConfigFile))
            {
                string json = reader.ReadToEnd();
                var jsonSerializerOptions = new JsonNodeOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                root = JsonNode.Parse(json, jsonSerializerOptions)!;
            }
        }
        string IConfiguration.this[string path]
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(path))
                        throw new InvalidPathException("Invalid path for configuration file.");

                    string[] pathSegments = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    var currentNode = root;

                    foreach (var pathSegment in pathSegments)
                    {
                        currentNode = currentNode[pathSegment]!;
                    }

                    return currentNode.ToString();
                }
                catch(NullReferenceException e)
                {
                    throw new InvalidPathException("Invalid path for configuration file.", e);
                }
            }
        }
    }
}
