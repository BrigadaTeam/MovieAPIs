using System.Collections.Generic;
using System.Threading;
using System;
using System.Runtime.CompilerServices;
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
    /// <summary>
    /// Client for working with the unofficial kinopoisk api.
    /// </summary>
    public class UnofficialKinopoiskApiClient : MovieApiClientBase
    {
        /// <summary>
        /// Constant elements of unofficial kinopoisk api methods.
        /// </summary>
        readonly UnofficialKinopoiskConstants constants;
        
        /// <summary>
        /// Constructor of an unofficial API client for movie search with api key parameter.
        /// </summary>
        /// <param name="apiKey">Key for unofficial kinopoisk api.</param>
        public UnofficialKinopoiskApiClient(string apiKey) : this(new UnofficialKinopoiskHttpClient(apiKey)) { }
        
        /// <summary>
        /// Constructor of an unofficial API client for movie search with custom HttpClient parameter.
        /// </summary>
        /// <param name="httpClient">Sending HTTP requests and receiving HTTP responses from a resource identified by a URI.</param>
        public UnofficialKinopoiskApiClient(IHttpClient httpClient) : base(httpClient, new UnofficialKinopoiskHttpInvalidCodeHandler())
        {
            constants = UnofficialKinopoiskConstants.GetUnofficialKinopoiskConstants(new NewtonsoftJsonSerializer());
        }

        #region Method for returning data from range page with range parameter.
        
        /// <summary>
        /// Get a list of movies from various tops and collections from page range.
        /// </summary>
        /// <param name="pageRange">Range of pages from which information is needed.</param>
        /// <param name="topType">Type of top or collection.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of movies.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmSearch> GetTopFilmsFromPageRangeAsync(Range pageRange, Tops topType = Tops.TOP_250_BEST_FILMS, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var filmSearch in GetTopFilmsFromPageRangeAsync(pageRange.Start.Value, pageRange.End.Value, topType, ct).ConfigureAwait(false))
            {
                yield return filmSearch;
            }
        }
        
        /// <summary>
        /// Get list of digital releases from page range.
        /// </summary>
        /// <param name="year">Release year.</param>
        /// <param name="month">Release month.</param>
        /// <param name="pageRange">Range of pages from which information is needed.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of digital releases.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmRelease> GetDigitalReleasesFromPageRangeAsync(int year, Months month, Range pageRange, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var filmRelease in GetDigitalReleasesFromPageRangeAsync(year, month, pageRange.Start.Value, pageRange.End.Value, ct).ConfigureAwait(false))
            {
                yield return filmRelease;
            }
        }

        /// <summary>
        /// Get list of movies by keywords from page range.
        /// </summary>
        /// <param name="keyword">Movie keyword.</param>
        /// <param name="pageRange">Range of pages from which information is needed.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of movies.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmSearch> GetFilmsByKeywordFromPageRangeAsync(string keyword, Range pageRange, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var filmSearch in GetFilmsByKeywordFromPageRangeAsync(keyword, pageRange.Start.Value, pageRange.End.Value, ct).ConfigureAwait(false))
            {
                yield return filmSearch;
            }
        }
        
        /// <summary>
        /// Get a list of movies by different filters from page range.
        /// </summary>
        /// <param name="pageRange">Range of pages from which information is needed.</param>
        /// <param name="countryId">Unofficial kinopoisk country id.</param>
        /// <param name="genreId">Unofficial kinopoisk genre id.</param>
        /// <param name="imdbId">Id for IMDb.</param>
        /// <param name="keyword">Movie keyword.</param>
        /// <param name="order">Order by rating, year, num vote.</param>
        /// <param name="type">Type of movie (film, TV show and etc).</param>
        /// <param name="ratingFrom">Minimum rating.</param>
        /// <param name="ratingTo">Maximum rating.</param>
        /// <param name="yearFrom">Minimum year.</param>
        /// <param name="yearTo">Maximum year.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of movies.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<Film> GetFilmsByFiltersFromPageRangeAsync(Range pageRange, int countryId = (int)Filter.ALL,
            int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
            MovieOrder order = MovieOrder.RATING, MovieType type = MovieType.ALL, int ratingFrom = 0, int ratingTo = 10,
            int yearFrom = 1000, int yearTo = 3000, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var film in GetFilmsByFiltersFromPageRangeAsync(countryId, genreId, imdbId, keyword, order, type, ratingFrom, ratingTo, yearFrom, yearTo,
                pageRange.Start.Value, pageRange.End.Value, ct).ConfigureAwait(false))
            {
                yield return film;
            }
        }
        
        /// <summary>
        /// Returns a list of viewer reviews from page range.
        /// </summary>
        /// <param name="id">Kinopoisk film id.</param>
        /// <param name="pageRange">Range of pages from which information is needed.</param>
        /// <param name="order">Order by date, rating and etc.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of viewer reviews.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<Review> GetViewerReviewsByIdFromPageRangeAsync(int id, Range pageRange,
            ReviewOrder order = ReviewOrder.DATE_DESC, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var review in GetViewerReviewsByIdFromPageRangeAsync(id, pageRange.Start.Value, pageRange.End.Value, order, ct).ConfigureAwait(false))
            {
                yield return review;
            }
        }
        
        /// <summary>
        /// Get movie related images from page range.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <param name="pageRange">Range of pages from which information is needed.</param>
        /// <param name="type">Image type.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of images</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<Image> GetImagesByIdFromPageRangeAsync(int id, Range pageRange,
            ImageType type = ImageType.STILL, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var image in GetImagesByIdFromPageRangeAsync(id, type, pageRange.Start.Value, pageRange.End.Value, ct).ConfigureAwait(false))
            {
                yield return image;
            }
        }
        
        /// <summary>
        /// Search for actors, directors, etc by name from page range.
        /// </summary>
        /// <param name="name">Person's name.</param>
        /// <param name="pageRange">Range of pages from which information is needed.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of information about a person.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmPersonInfo> GetPersonByNameFromPageRangeAsync(string name, Range pageRange, [EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var filmPersonInfo in GetPersonByNameFromPageRangeAsync(name, pageRange.Start.Value, pageRange.End.Value, ct).ConfigureAwait(false))
            {
                yield return filmPersonInfo;
            }
        }
        #endregion

        #region Method for returning data from range page without range parameter.
        
        /// <summary>
        /// Get a list of movies from various tops and collections from start to end page.
        /// </summary>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <param name="topType">Type of top or collection.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of movies.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmSearch> GetTopFilmsFromPageRangeAsync(int fromPage = -1, int toPage = -1, Tops topType = Tops.TOP_250_BEST_FILMS, [EnumeratorCancellation] CancellationToken ct = default)
        {  
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString()
            };
            string path = $"{constants.TopUrlV22}";
            var response = await GetTopFilmsAsync(topType, ct: ct).ConfigureAwait(false);
            int requestsCountInSecond = 5;
            await foreach (var filmSearch in GetResponsesDataFromPageRangeAsync<FilmSearch>(path, queryParams, requestsCountInSecond, fromPage, toPage, constants.NumberFirstPage, response.PagesCount, ct).ConfigureAwait(false))
            {
                yield return filmSearch;
            }
        }
        
        /// <summary>
        /// Get list of digital releases from start to end page.
        /// </summary>
        /// <param name="year">Release year.</param>
        /// <param name="month">Release month.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of digital releases.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmRelease> GetDigitalReleasesFromPageRangeAsync(int year, Months month, int fromPage = -1, int toPage = -1, [EnumeratorCancellation] CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString()
            };
            string path = $"{constants.ReleasesUrlV21}";
            var response = await GetDigitalReleasesAsync(year, month, ct: ct).ConfigureAwait(false);
            int requestCountInSecond = 5;
            await foreach (var filmRelease in GetResponsesDataFromPageRangeAsync<FilmRelease>(path, queryParams, requestCountInSecond, fromPage, toPage, constants.NumberFirstPage, response.PagesCount, ct).ConfigureAwait(false))
            {
                yield return filmRelease;
            }
        }
        
        /// <summary>
        /// Get list of movies by keywords from start to end page.
        /// </summary>
        /// <param name="keyword">Movie keyword.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of movies.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmSearch> GetFilmsByKeywordFromPageRangeAsync(string keyword, int fromPage = -1, int toPage = -1, [EnumeratorCancellation] CancellationToken ct = default)
        {
            string path = $"{constants.SearchByKeywordUrlV21}";
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword
            };
            var response = await GetFilmsByKeywordAsync(keyword, ct: ct).ConfigureAwait(false);
            int requestsCountInSecond = 5;
            await foreach (var filmSearch in GetResponsesDataFromPageRangeAsync<FilmSearch>(path, queryParams, requestsCountInSecond, fromPage, toPage, constants.NumberFirstPage, response.PagesCount, ct).ConfigureAwait(false))
            {
                yield return filmSearch;
            }
        }

        /// <summary>
        /// Get a list of movies by different filters from start to end page.
        /// </summary>
        /// <param name="countryId">Unofficial kinopoisk country id.</param>
        /// <param name="genreId">Unofficial kinopoisk genre id.</param>
        /// <param name="imdbId">Id for IMDb.</param>
        /// <param name="keyword">Movie keyword.</param>
        /// <param name="order">Order by rating, year, num vote.</param>
        /// <param name="type">Type of movie (film, TV show and etc).</param>
        /// <param name="ratingFrom">Minimum rating.</param>
        /// <param name="ratingTo">Maximum rating.</param>
        /// <param name="yearFrom">Minimum year.</param>
        /// <param name="yearTo">Maximum year.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of movies.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<Film> GetFilmsByFiltersFromPageRangeAsync(int countryId = (int)Filter.ALL,
            int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
            MovieOrder order = MovieOrder.RATING, MovieType type = MovieType.ALL, int ratingFrom = 0, int ratingTo = 10,
            int yearFrom = 1000, int yearTo = 3000, int fromPage = -1, int toPage = -1, [EnumeratorCancellation] CancellationToken ct = default)
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
                ["keyword"] = keyword
            };
            string path = $"{constants.FiltersUrlV22}";
            var response = await GetFilmsByFiltersAsync(countryId: countryId, genreId: genreId, imdbId: imdbId, keyword: keyword, order: order,
                type: type, ratingFrom: ratingFrom, ratingTo: ratingTo, yearFrom: yearFrom, yearTo: yearTo, ct: ct).ConfigureAwait(false);
            int requestCountInSecond = 5;
            await foreach (var film in GetResponsesDataFromPageRangeAsync<Film>(path, queryParams, requestCountInSecond, fromPage, toPage, constants.NumberFirstPage, response.PagesCount, ct).ConfigureAwait(false))
            {
                yield return film;
            }
        }
        
        /// <summary>
        /// Returns a list of viewer reviews from start to end page.
        /// </summary>
        /// <param name="id">Kinopoisk film id.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <param name="order">Order by date, rating and etc.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of viewer reviews.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<Review> GetViewerReviewsByIdFromPageRangeAsync(int id, int fromPage = -1, int toPage = -1,
            ReviewOrder order = ReviewOrder.DATE_DESC, [EnumeratorCancellation] CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["order"] = order.ToString()
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ReviewsPathSegment}";
            var response = await GetViewerReviewsByIdAsync(id, order: order, ct: ct).ConfigureAwait(false);
            int requestsCountInSecond = 20;
            await foreach (var review in GetResponsesDataFromPageRangeAsync<Review>(path, queryParams, requestsCountInSecond, fromPage, toPage, constants.NumberFirstPage, response.PagesCount, ct).ConfigureAwait(false))
            {
                yield return review;
            }
        }
        
        /// <summary>
        /// Get movie related images from start to end page.
        /// </summary>
        /// <param name="id">Kinopoisk film id.</param>
        /// <param name="type">Image type.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of images.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<Image> GetImagesByIdFromPageRangeAsync(int id,
            ImageType type = ImageType.STILL, int fromPage = -1, int toPage = -1, [EnumeratorCancellation] CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = type.ToString()
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ImagesPathSegment}";
            var response = await GetImagesByIdAsync(id, type, ct: ct).ConfigureAwait(false);
            int requestCountInSecond = 20;
            await foreach (var image in GetResponsesDataFromPageRangeAsync<Image>(path, queryParams, requestCountInSecond, fromPage, toPage, constants.NumberFirstPage, response.PagesCount, ct).ConfigureAwait(false))
            {
                yield return image;
            }
        }
        
        /// <summary>
        /// Search for actors, directors, etc by name from start to end page.
        /// </summary>
        /// <param name="name">Person's name.</param>
        /// <param name="fromPage">Start page.</param>
        /// <param name="toPage">End page.</param>
        /// <returns>Returns an enumerator that iterates asynchronously through the collection of information about a person.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async IAsyncEnumerable<FilmPersonInfo> GetPersonByNameFromPageRangeAsync(string name, int fromPage = -1, int toPage = -1, [EnumeratorCancellation] CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["name"] = name
            };
            string path = $"{constants.PersonsUrlV1}";
            var response = await GetPersonByNameAsync(name, ct: ct).ConfigureAwait(false);
            int requestCountInSecond = 5;
            await foreach (var filmPersonInfo in GetResponsesDataFromPageRangeAsync<FilmPersonInfo>(path, queryParams, requestCountInSecond, fromPage, toPage, constants.NumberFirstPage, response.PagesCount, ct).ConfigureAwait(false))
            {
                yield return filmPersonInfo;
            }
        }
        #endregion

        #region Method for returning data without page parameter.
        
        /// <summary>
        /// Get movie data by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a film response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<Film> GetFilmByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}";
            return await GetResponseDataAsync<Film>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns a list of country and genre IDs that have been sold in /api/v2.2/films.
        /// </summary>
        /// <returns>A task object with a genres and countries response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<GenresAndCountries> GetGenresAndCountriesAsync(CancellationToken ct = default)
        {
            string path = $"{constants.FiltersUrlV22}";
            return await GetResponseDataAsync<GenresAndCountries>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of similar movies by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a related films response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponse<RelatedFilm>> GetRelatedFilmsAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SimilarsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<RelatedFilm>>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get data about facts and errors in movies by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a facts and errors response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponse<FactsAndMistakes>> GetFilmFactsAndMistakesAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.FactsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<FactsAndMistakes>>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get rental data in different countries by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a film distributions response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponse<FilmDistributionsResponseItems>> GetFilmDistributionsAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.DistributionsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<FilmDistributionsResponseItems>>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get sequels and prequels by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a film response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<FilmSearch[]> GetSequelsAndPrequelsByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV21}/{id}/{constants.SequelsAndPrequelsPathSegment}";
            return await GetResponseDataAsync<FilmSearch[]>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get budget and collection information by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a monetization info response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponse<MonetizationInfo>> GetBoxOfficeByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.BoxOfficePathSegment}";
            return await GetResponseDataAsync<ItemsResponse<MonetizationInfo>>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get season data for the series by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a season response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponse<Season>> GetSeasonsDataByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SeasonsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<Season>>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get data about actors, directors, etc. by kinopoisk id.
        /// </summary>
        /// <param name="filmId">Kinopoisk id.</param>
        /// <returns>A task object with a staff response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<Staff[]> GetStaffByFilmIdAsync(int filmId, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["filmId"] = filmId.ToString(),
            };
            string path = $"{constants.StaffUrlV1}";
            return await GetResponseDataAsync<Staff[]>(path, ct, queryParams).ConfigureAwait(false);
        }

        /// <summary>
        /// Get list of film premieres.
        /// </summary>
        /// <param name="year">Release year.</param>
        /// <param name="month">Release month.</param>
        /// <returns>A task object with a film premiere response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
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

        /// <summary>
        /// Get trailers, teasers, videos for the film by kinopoisk id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a video response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponse<Video>> GetTrailersAndTeasersByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.VideosPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<Video>>(path, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get film awards data by kinopoisk film id.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <returns>A task object with a nomination response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponse<Nomination>> GetAwardsByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.AwardsPathSegment}";
            return await GetResponseDataAsync<ItemsResponse<Nomination>>(path, ct).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Get information about a specific person by kinopoisk id.
        /// </summary>
        /// <param name="personId">Person kinopoisk id.</param>
        /// <returns>A task object with a person response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<Person> GetStaffByPersonIdAsync(int personId, CancellationToken ct = default)
        {
            string path = $"{constants.StaffUrlV1}/{personId}";
            return await GetResponseDataAsync<Person>(path, ct).ConfigureAwait(false);
        }

        #endregion

        #region Method for returning data from one page with page parameter.
        
        /// <summary>
        /// Get list of digital releases.
        /// </summary>
        /// <param name="year">Release year.</param>
        /// <param name="month">Release month.</param>
        /// <param name="page">Page number.</param>
        /// <returns>A task object with a film release response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
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

        /// <summary>
        /// Get movie list by keyword.
        /// </summary>
        /// <param name="keyword">Movie keyword.</param>
        /// <param name="page">Page number.</param>
        /// <returns>A task object with a film response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
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

        /// <summary>
        /// Get a list of movies by various filters.
        /// </summary>
        /// <param name="countryId">Unofficial kinopoisk country id.</param>
        /// <param name="genreId">Unofficial kinopoisk genre id.</param>
        /// <param name="imdbId">Id for IMDb.</param>
        /// <param name="keyword">Movie keyword.</param>
        /// <param name="order">Order by rating, year, num vote.</param>
        /// <param name="type">Type of movie (film, TV show and etc).</param>
        /// <param name="ratingFrom">Minimum rating.</param>
        /// <param name="ratingTo">Maximum rating.</param>
        /// <param name="yearFrom">Minimum year.</param>
        /// <param name="yearTo">Maximum year.</param>
        /// <param name="page">Page number.</param>
        /// <returns>A task object with a film response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
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

        /// <summary>
        /// Get movies from various tops and collections.
        /// </summary>
        /// <param name="topType">Type of top or collection.</param>
        /// <param name="page">Page number.</param>
        /// <returns>A task object with a film response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
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

        /// <summary>
        /// Returns a list of paginated viewer reviews.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <param name="page">Page number.</param>
        /// <param name="order">Order by date, rating and etc.</param>
        /// <returns>A task object with a review response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ViewerReviewsResponse<Review>> GetViewerReviewsByIdAsync(int id, int page = 1,
            ReviewOrder order = ReviewOrder.DATE_DESC, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["order"] = order.ToString(),
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ReviewsPathSegment}";
            return await GetResponseDataAsync<ViewerReviewsResponse<Review>>(path, ct, queryParams).ConfigureAwait(false);
        }
       
        /// <summary>
        /// Get images associated with the movie with pagination. Each page contains no more than 20 films.
        /// </summary>
        /// <param name="id">Kinopoisk id.</param>
        /// <param name="type">Image type.</param>
        /// <param name="page">Page number.</param>
        /// <returns>A task object with a image response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
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

        /// <summary>
        /// Search for actors, directors, etc. by name. One page can contain up to 50 items in items.
        /// </summary>
        /// <param name="name">Person name.</param>
        /// <param name="page">Page number.</param>
        /// <returns>A task object with a person info response model.</returns>
        /// <exception cref="HttpRequestException">An exception thrown when a http request responds with a status code other than successful.</exception>
        public async Task<ItemsResponseWithPagesCount<FilmPersonInfo>> GetPersonByNameAsync(string name, int page = 1, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["name"] = name,
                ["page"] = page.ToString()
            };
            string path = $"{constants.PersonsUrlV1}";
            return await GetResponseDataAsync<ItemsResponseWithPagesCount<FilmPersonInfo>>(path, ct, queryParams).ConfigureAwait(false);
        }
        #endregion
    }
}
