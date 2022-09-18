using MovieAPIs.Common.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace MovieAPIs.UnofficialKinopoiskApi.Configuration
{
    /// <summary>
    /// Constant elements of apis methods.
    /// </summary>
    internal class UnofficialKinopoiskConstants
    {
        /// <summary>
        ///  Constant elements of UnofficialKinopoisk api methods.
        /// </summary>
        static UnofficialKinopoiskConstants unofficialKinopoiskConstants;
        static object syncRoot = new();
        private UnofficialKinopoiskConstants() { }

        /// <summary>
        /// Getting constants from a config file.
        /// </summary>
        /// <param name="serializer">Serializer for config file.</param>
        /// <returns>Constant elements of apis methods.</returns>
        public static UnofficialKinopoiskConstants GetUnofficialKinopoiskConstants(ISerializer serializer)
        {
            if(unofficialKinopoiskConstants == null)
            {
                lock (syncRoot)
                {
                    if(unofficialKinopoiskConstants == null)
                    {
                        using (var reader = new StreamReader(Path.Combine("UnofficialKinopoiskApi", "Configuration", "UnofficialKinopoiskConfiguration.json")))
                        {
                            string json = reader.ReadToEnd();
                            unofficialKinopoiskConstants = serializer.Deserialize<UnofficialKinopoiskConstants>(json);
                        }
                    }
                }
            }

            return unofficialKinopoiskConstants;
        }

        [JsonProperty]
        public readonly string FilmsUrlV22;

        [JsonProperty]
        public readonly string FiltersUrlV22;

        [JsonProperty]
        public readonly string PremieresUrlV22;

        [JsonProperty]
        public readonly string TopUrlV22;

        [JsonProperty]
        public readonly string SearchByKeywordUrlV21;

        [JsonProperty]
        public readonly string FilmsUrlV21;

        [JsonProperty]
        public readonly string ReleasesUrlV21;

        [JsonProperty]
        public readonly string StaffUrlV1;

        [JsonProperty]
        public readonly string PersonsUrlV1;

        [JsonProperty]
        public readonly string SeasonsPathSegment;

        [JsonProperty]
        public readonly string FactsPathSegment;

        [JsonProperty]
        public readonly string DistributionsPathSegment;

        [JsonProperty]
        public readonly string BoxOfficePathSegment;

        [JsonProperty]
        public readonly string AwardsPathSegment;

        [JsonProperty]
        public readonly string VideosPathSegment;

        [JsonProperty]
        public readonly string SimilarsPathSegment;

        [JsonProperty]
        public readonly string ImagesPathSegment;

        [JsonProperty]
        public readonly string ReviewsPathSegment;

        [JsonProperty]
        public readonly string SequelsAndPrequelsPathSegment;

        [JsonProperty]
        public readonly int NumberFirstPage;
    }
}
