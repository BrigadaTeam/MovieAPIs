using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MovieAPIs.Utils
{
    internal interface IConfiguration
    {
        internal string this[string path] { get; }
    }

    internal class InvalidPathException : Exception
    {
        public InvalidPathException(string message) : base(message)
        {

        }
    }

    internal class JsonConfiguration : IConfiguration
    {
        readonly JsonDocument document;
        internal JsonConfiguration(string pathToConfigFile)
        {
            using (var reader = new StreamReader(pathToConfigFile))
            {
                string json = reader.ReadToEnd();
                document = JsonDocument.Parse(json);
            }
        }
        string IConfiguration.this[string path]
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(path))
                        throw new InvalidPathException("Invalid path");

                    string[] pathSegments = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    var currentNode = document.RootElement;

                    foreach (var pathSegment in pathSegments)
                    {
                        currentNode = currentNode.EnumerateObject()
                            .FirstOrDefault(x => string.Compare(x.Name, pathSegment, StringComparison.OrdinalIgnoreCase) == 0)
                            .Value;
                    }

                    if (currentNode.ValueKind != JsonValueKind.String)
                        throw new InvalidPathException("Invalid path");

                    return currentNode.ToString()!;
                }
                catch(InvalidOperationException)
                {
                    throw new InvalidPathException("Invalid path");
                }
            }
        }
    }
}
