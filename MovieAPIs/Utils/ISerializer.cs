using System.IO;

namespace MovieAPIs.Utils
{
    internal interface ISerializer
    {
        internal T Deserialize<T>(string json);
    }
}
