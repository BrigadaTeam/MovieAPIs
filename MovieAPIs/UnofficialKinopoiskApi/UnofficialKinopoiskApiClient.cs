using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;
using MovieAPIs.Common;
using MovieAPIs.Common.Http;
using MovieAPIs.UnofficialKinopoiskApi.Configuration;
using MovieAPIs.UnofficialKinopoiskApi.Http;
using MovieAPIs.Common.Serialization;
using MovieAPIs.UnofficialKinopoiskApi.Models;
using MovieAPIs.Common.Responses;

namespace MovieAPIs.UnofficialKinopoiskApi
{
    public class UnofficialKinopoiskApiClient : MovieApiClientBase
    {
        readonly UnofficialKinopoiskConstants constants;
        public UnofficialKinopoiskApiClient(string apiKey) : this(new UnofficialKinopoiskHttpClient(apiKey)) { }

        public UnofficialKinopoiskApiClient(IHttpClient httpClient) : base(httpClient, new UnofficialKinopoiskHttpInvalidCodeHandler())
        {
            constants = UnofficialKinopoiskConstants.GetUnofficialKinopoiskConstants(new NewtonsoftJsonSerializer());
        }
        public async IAsyncEnumerable<FilmSearch> GetTopFilmsFromPageRangeAsync(Range pageRange, Tops topType = Tops.TOP_250_BEST_FILMS, CancellationToken ct = default)
        {
            await foreach (var filmSearch in GetTopFilmsFromPageRangeAsync(pageRange.Start.Value, pageRange.End.Value, topType, ct).ConfigureAwait(false))
            {
                yield return filmSearch;
            }
        }
        public async IAsyncEnumerable<FilmSearch> GetTopFilmsFromPageRangeAsync(int fromPage = -1, int toPage = -1, Tops topType = Tops.TOP_250_BEST_FILMS, CancellationToken ct = default)
        {  
            fromPage = fromPage == -1 ? constants.NumberFirstPage : fromPage;
            toPage = toPage == -1 ? await GetPagesCountAsync(GetTopFilmsAsync(topType, ct: ct)).ConfigureAwait(false) : toPage;
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString()
            };
            string path = $"{constants.TopUrlV22}";
            await foreach (var filmSearch in GetResponsesDataFromPageRangeAsync<FilmSearch>(path, queryParams, 5, fromPage, toPage, ct).ConfigureAwait(false))
            {
                yield return filmSearch;
            }
        }
        
        public async Task<Film> GetFilmByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}";
            return await GetResponseDataAsync<Film>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponseWithPagesCount<FilmSearch>> GetFilmsByKeywordAsync(string keyword, int page = 1, CancellationToken ct = default)
        {
            string path = $"{constants.SearchByKeywordUrlV21}";
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            return await GetResponseDataAsync<ItemsResponseWithPagesCount<FilmSearch>>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<GenresAndCountries> GetGenresAndCountriesAsync(CancellationToken ct = default)
        {
            string path = $"{constants.FiltersUrlV22}";
            return await GetResponseDataAsync<GenresAndCountries>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponseWithPagesCount<Film>> GetFilmsByFiltersAsync(int countryId = (int)Filter.ALL,
            int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
            MovieOrder order = MovieOrder.RATING, MovieType type = MovieType.ALL, int ratingFrom = 0, int ratingTo = 10,
            int yearFrom = 1000, int yearTo = 3000, int page = 1, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["countries"] = countryId == (int)Filter.ALL ? "" : countryId.ToString(),
                ["genres"] = genreId == (int)Filter.ALL ? "" : genreId.ToString(),
                ["order"] = order.ToString(),
                ["type"] = type.ToString(),
                ["ratingFrom"] = ratingFrom.ToString(),
                ["ratingTo"] = ratingTo.ToString(),
                ["yearFrom"] = yearFrom.ToString(),
                ["yearTo"] = yearTo.ToString(),
                ["imdbId"] = imdbId,
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            string path = $"{constants.FiltersUrlV22}";
            return await GetResponseDataAsync<ItemsResponseWithPagesCount<Film>>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<RelatedFilm>> GetRelatedFilmsAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SimilarsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<RelatedFilm>>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponseWithPagesCount<FilmSearch>> GetTopFilmsAsync(Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            string path = $"{constants.TopUrlV22}";
            return await GetResponseDataAsync<ItemsResponseWithPagesCount<FilmSearch>>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<FactsAndMistakes>> GetFilmFactsAndMistakesAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.FactsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<FactsAndMistakes>>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<FilmDistributionsResponseItems>> GetFilmDistributionsAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.DistributionsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<FilmDistributionsResponseItems>>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponseWithPagesCount<FilmRelease>> GetDigitalReleasesAsync(int year, Months month, 
             int page = 1, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
                ["page"] = page.ToString()
            };
            string path = $"{constants.ReleasesUrlV21}";
            return await GetResponseDataAsync<ItemsResponseWithPagesCount<FilmRelease>>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<FilmSearch[]> GetSequelsAndPrequelsByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV21}/{id}/{constants.SequelsAndPrequelsPathSegment}";
            return await GetResponseDataAsync<FilmSearch[]>(path, ct).ConfigureAwait(false);
        }

        public async Task<ViewerReviews> GetViewerReviewsByIdAsync(int id, int page = 1,
            ReviewOrder order = ReviewOrder.DATE_DESC, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["order"] = order.ToString(),
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ReviewsPathSegment}";
            return await GetResponseDataAsync<ViewerReviews>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<MonetizationInfo>> GetBoxOfficeByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.BoxOfficePathSegment}";
            return await GetResponseDataAsync<ItemsResponse<MonetizationInfo>>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<Season>> GetSeasonsDataByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SeasonsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<Season>>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponseWithPagesCount<Image>> GetImagesByIdAsync(int id, 
            ImageType type = ImageType.STILL, int page = 1, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["type"] = type.ToString(),
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ImagesPathSegment}";
            return await GetResponseDataAsync<ItemsResponseWithPagesCount<Image>>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<Staff[]> GetStaffByFilmIdAsync(int filmId, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["filmId"] = filmId.ToString(),
            };
            string path = $"{constants.StaffUrlV1}";
            return await GetResponseDataAsync<Staff[]>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<FilmPremiere>> GetPremieresListAsync(int year, Months month, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
            };
            string path = $"{constants.PremieresUrlV22}";
            return await GetResponseDataAsync<ItemsResponse<FilmPremiere>>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<Video>> GetTrailersAndTeasersByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.VideosPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<Video>>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<Nomination>> GetAwardsByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.AwardsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<Nomination>>(path, ct).ConfigureAwait(false);
        }

        public async Task<ItemsResponse<FilmPersonInfo>> GetPersonByNameAsync(string name, int page = 1, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["name"] = name,
                ["page"] = page.ToString()
            };
            string path = $"{constants.PersonsUrlV1}";
            return await GetResponseDataAsync<ItemsResponse<FilmPersonInfo>>(path, ct, queryParams).ConfigureAwait(false);
        }

        public async Task<Person> GetStaffByPersonIdAsync(int personId, CancellationToken ct = default)
        {
            string path = $"{constants.StaffUrlV1}/{personId}";
            return await GetResponseDataAsync<Person>(path, ct).ConfigureAwait(false);
        }
    }
}
