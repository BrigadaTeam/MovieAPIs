using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MovieAPIs.Utils
{
    internal interface IConfiguration
    {
        internal string this[string path] { get; }
    }

    internal class InvalidPathException : Exception
    {
        public InvalidPathException(string message) : base(message) { }
    }

    internal class JsonConfiguration : IConfiguration
    {
        readonly JObject root;
        internal JsonConfiguration(string pathToConfigFile)
        {
            using (var reader = new StreamReader(pathToConfigFile))
            {
                string json = reader.ReadToEnd();

                root = JObject.Parse(json);
            }
        }
        string IConfiguration.this[string path]
        {
            get
            {
                    if (string.IsNullOrEmpty(path))
                        throw new InvalidPathException("Invalid path for configuration file.");

                    string[] pathSegments = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    var jsonPath = new StringBuilder("$");
                    foreach(var pathSegment in pathSegments)
                    {
                        jsonPath.Append($".{pathSegment}");
                    }
                    var configValue = root.SelectToken(jsonPath.ToString());
                    if(configValue == null)
                        throw new InvalidPathException("Invalid path for configuration file.");

                    return configValue.ToString();
            }
        }
    }
}
