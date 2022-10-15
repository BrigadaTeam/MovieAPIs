using Newtonsoft.Json;

namespace MovieAPIs.Common.Serialization
{
    /// <summary>
    /// Interface for serialization.
    /// </summary>
    internal interface ISerializer
    {
        /// <summary>
        /// Deserializing a json string into an object.
        /// </summary>
        /// <param name="json">Json string.</param>
        /// <typeparam name="T">Deserialize type.</typeparam>
        /// <returns>Deserialize object.</returns>
        internal T Deserialize<T>(string json);
    }
    
    /// <summary>
    /// Newtonsoft Json serializer.
    /// </summary>
    internal class NewtonsoftJsonSerializer : ISerializer 
    {
        /// <summary>
        /// Deserializing a json string into an object using a Newtonsoft Json serializer.
        /// </summary>
        /// <param name="json">Json string.</param>
        /// <typeparam name="T">Deserialize type.</typeparam>
        /// <returns>Deserialize object.</returns>
        T ISerializer.Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}
