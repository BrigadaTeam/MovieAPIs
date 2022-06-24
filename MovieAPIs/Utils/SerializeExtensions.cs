using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using MovieAPIs.ResponseModels;
using MovieAPIs.Utils;
using System.Text;

namespace MovieAPIs.Utils
{
    static internal class SerializeExtensions
    {
        private static JsonSerializerOptions jsonSerializerOptions;

        static SerializeExtensions()
        {
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }
        static internal FilmTopResponse GetTopFilmsAllPages(List<string> requestResponseBody)
        {
            var responses = new List<FilmTopResponse>();
        
            foreach (var curData in requestResponseBody)
            {
                responses.Add(JsonSerializer.Deserialize<FilmTopResponse>(curData, jsonSerializerOptions));
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

        static internal FilmSearchResponse GetFilmsByKeywordAllPages(List<string> requestResponseBody)
        {
            var responses = new List<FilmSearchResponse>();

            foreach (var curData in requestResponseBody)
            {
                responses.Add(JsonSerializer.Deserialize<FilmSearchResponse>(curData, jsonSerializerOptions));
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

        static internal FilmSearchByFilterResponse GetFilmsByFiltersAllPages(List<string> requestResponseBody)
        {
            var responses = new List<FilmSearchByFilterResponse>();

            foreach (var curData in requestResponseBody)
            {
                responses.Add(JsonSerializer.Deserialize<FilmSearchByFilterResponse>(curData, jsonSerializerOptions));
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
    }
}
