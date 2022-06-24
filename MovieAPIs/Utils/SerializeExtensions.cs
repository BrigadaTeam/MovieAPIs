using System.Text.Json;
using MovieAPIs.ResponseModels;
using System.Text;

namespace MovieAPIs.Utils
{
    static internal class SerializeExtensions
    {
        private static ISerializer serializer;
        static SerializeExtensions()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            serializer = new DotNetJsonSerializer(jsonSerializerOptions);
        }

        internal static FilmTopResponse GetTopFilmsAllPages(List<string> requestResponseBody)
        {
            var responses = new List<FilmTopResponse>();
        
            foreach (var curData in requestResponseBody)
            {
                responses.Add(serializer.Deserialize<FilmTopResponse>(StringToStream(curData)));
            }

            var filmsResponse = new FilmTopResponse()
            {
                PagesCount = responses.First().PagesCount,
                Films = Array.Empty<FilmSearchResponseFilm>()
            };

            foreach (var curData in responses)
            {
                filmsResponse.Films = filmsResponse.Films.Concat(curData.Films).ToArray();
            }
            return filmsResponse;
        }

        internal static FilmSearchResponse GetFilmsByKeywordAllPages(List<string> requestResponseBody)
        {
            var responses = new List<FilmSearchResponse>();

            foreach (var curData in requestResponseBody)
            {
                responses.Add(serializer.Deserialize<FilmSearchResponse>(StringToStream(curData)));
            }

            var filmsResponse = new FilmSearchResponse()
            {
                Keyword = responses.First().Keyword,
                PagesCount = responses.First().PagesCount,
                Films = Array.Empty<FilmSearchResponseFilm>()
            };

            foreach (var curData in responses)
            {
                filmsResponse.Films = filmsResponse.Films.Concat(curData.Films).ToArray();
            }

            return filmsResponse;
        }

        internal static FilmSearchByFilterResponse GetFilmsByFiltersAllPages(List<string> requestResponseBody)
        {
            var responses = new List<FilmSearchByFilterResponse>();

            foreach (var curData in requestResponseBody)
            {
                responses.Add(serializer.Deserialize<FilmSearchByFilterResponse>(StringToStream(curData)));
            }

            var filmsResponse = new FilmSearchByFilterResponse()
            {
                TotalPages = responses.First().TotalPages,
                Films = Array.Empty<Film>()
            };

            foreach (var curData in responses)
            {
                filmsResponse.Films = filmsResponse.Films.Concat(curData.Films).ToArray();
            }

            return filmsResponse;
        }

        private static Stream StringToStream(string str)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);

            return new MemoryStream(byteArray);
        }
    }
}
