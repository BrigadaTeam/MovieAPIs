namespace MovieAPIs.Utils
{
    internal interface ISerializer
    {
        internal T Deserialize<T>(Stream stream);
    }
}
