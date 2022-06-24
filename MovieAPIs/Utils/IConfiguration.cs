
namespace MovieAPIs.Utils
{
    internal interface IConfiguration
    {
        internal string this[string path] { get; }
    }
}
