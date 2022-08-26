using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;
using MovieAPIs.Configuration;
using MovieAPIs.Models;
using MovieAPIs.Utils;

namespace MovieAPIs
{
    public class UnofficialKinopoiskApiClient : MovieApiClientBase
    {
        readonly UnofficialKinopoiskConstants constants;
        public UnofficialKinopoiskApiClient(string apiKey) : this(new InternalHttpClient(apiKey), new NewtonsoftJsonSerializer()) { }

        public UnofficialKinopoiskApiClient(string apiKey, ISerializer serializer) : this(new InternalHttpClient(apiKey), serializer) { }

        public UnofficialKinopoiskApiClient(IHttpClient httpClient) : this(httpClient, new NewtonsoftJsonSerializer()) { }

        public UnofficialKinopoiskApiClient(IHttpClient httpClient, ISerializer serializer) : base(httpClient, serializer)
        {
            constants = UnofficialKinopoiskConstants.GetUnofficialKinopoiskConstants(new NewtonsoftJsonSerializer());
        }

        public async Task<Film> GetFilmByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}";
            return await GetResponseDataAsync<Film>(path, ct);
        }
        
        public IAsyncEnumerable<FilmSearch> GetTopFilmsFromPageRangeAsync(Range pageRange, CancellationToken ct = default, Tops topType = Tops.TOP_250_BEST_FILMS)
        {
            return GetTopFilmsFromPageRangeAsync(ct, pageRange.Start.Value, pageRange.End.Value, topType);
        }
        
        public IAsyncEnumerable<FilmSearch> GetTopFilmsFromPageRangeAsync(CancellationToken ct = default, int fromPage = -1, int toPage = -1, Tops topType = Tops.TOP_250_BEST_FILMS)
        {  
            fromPage = fromPage == -1 ? constants.NumberFirstPage : fromPage;
            toPage = toPage == -1 ? GetPagesCount(GetTopFilmsAsync(ct, topType)) : toPage;
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString()
            };
            string path = $"{constants.TopUrlV22}";
            return GetResponsesDataFromPageRangeAsync<FilmSearch>(path, queryParams, 5, fromPage, toPage, ct);
        }
        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetFilmsByKeywordAsync(string keyword, CancellationToken ct = default, int page = 1)
        {
            string path = $"{constants.SearchByKeywordUrlV21}";
            var queryParams = new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["page"] = page.ToString()
            };
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<FilmSearch>>(path, ct, queryParams);
        }

        public async Task<GenresAndCountriesResponse> GetGenresAndCountriesAsync(CancellationToken ct = default)
        {
            string path = $"{constants.FiltersUrlV22}";
            return await GetResponseDataAsync<GenresAndCountriesResponse>(path, ct);
        }

        public async Task<FilmsResponseWithPagesCount<Film>> GetFilmsByFiltersAsync(CancellationToken ct = default, int countryId = (int)Filter.ALL,
            int genreId = (int)Filter.ALL, string imdbId = "", string keyword = "",
            MovieOrder order = MovieOrder.RATING, MovieType type = MovieType.ALL, int ratingFrom = 0, int ratingTo = 10,
            int yearFrom = 1000, int yearTo = 3000, int page = 1)
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
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<Film>>(path, ct, queryParams);
        }

        public async Task<FilmsResponse<RelatedFilm>> GetRelatedFilmsAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SimilarsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<RelatedFilm>>(path, ct);
        }

        public async Task<FilmsResponseWithPagesCount<FilmSearch>> GetTopFilmsAsync(CancellationToken ct = default,
            Tops topType = Tops.TOP_250_BEST_FILMS, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = topType.ToString(),
                ["page"] = page.ToString()
            };
            string path = $"{constants.TopUrlV22}";
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<FilmSearch>>(path, ct, queryParams);
        }

        public async Task<FilmsResponse<FactsAndMistakes>> GetFilmFactsAndMistakesAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.FactsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<FactsAndMistakes>>(path, ct);
        }

        public async Task<FilmsResponse<FilmDistributionsResponseItems>> GetFilmDistributionsAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.DistributionsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<FilmDistributionsResponseItems>>(path, ct);
        }

        public async Task<FilmsResponseWithPagesCount<FilmRelease>> GetDigitalReleasesAsync(int year, Months month, 
            CancellationToken ct = default, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
                ["page"] = page.ToString()
            };
            string path = $"{constants.ReleasesUrlV21}";
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<FilmRelease>>(path, ct, queryParams);
        }

        public async Task<FilmSearch[]> GetSequelsAndPrequelsByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV21}/{id}/{constants.SequelsAndPrequelsPathSegment}";
            return await GetResponseDataAsync<FilmSearch[]>(path, ct);
        }

        public async Task<ViewerReviewsResponse> GetViewerReviewsByIdAsync(int id, CancellationToken ct = default, int page = 1,
            ReviewOrder order = ReviewOrder.DATE_DESC)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["order"] = order.ToString(),
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ReviewsPathSegment}";
            return await GetResponseDataAsync<ViewerReviewsResponse>(path, ct, queryParams);
        }

        public async Task<FilmsResponse<MonetizationInfo>> GetBoxOfficeByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.BoxOfficePathSegment}";
            return await GetResponseDataAsync<FilmsResponse<MonetizationInfo>>(path, ct);
        }

        public async Task<FilmsResponse<Season>> GetSeasonsDataByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.SeasonsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<Season>>(path, ct);
        }

        public async Task<FilmsResponseWithPagesCount<ImageResponse>> GetImagesByIdAsync(int id, CancellationToken ct = default,
            ImageType type = ImageType.STILL, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["type"] = type.ToString(),
            };
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.ImagesPathSegment}";
            return await GetResponseDataAsync<FilmsResponseWithPagesCount<ImageResponse>>(path, ct, queryParams);
        }

        public async Task<StaffResponse[]> GetStaffByFilmIdAsync(int filmId, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["filmId"] = filmId.ToString(),
            };
            string path = $"{constants.StaffUrlV1}";
            return await GetResponseDataAsync<StaffResponse[]>(path, ct, queryParams);
        }

        public async Task<FilmsResponse<FilmPremiere>> GetPremieresListAsync(int year, Months month, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["year"] = year.ToString(),
                ["month"] = month.ToString(),
            };
            string path = $"{constants.PremieresUrlV22}";
            return await GetResponseDataAsync<FilmsResponse<FilmPremiere>>(path, ct, queryParams);
        }

        public async Task<FilmsResponse<Video>> GetTrailersAndTeasersByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.VideosPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<Video>>(path, ct);
        }

        public async Task<FilmsResponse<Nomination>> GetAwardsByIdAsync(int id, CancellationToken ct = default)
        {
            string path = $"{constants.FilmsUrlV22}/{id}/{constants.AwardsPathSegment}";
            return await GetResponseDataAsync<FilmsResponse<Nomination>>(path, ct);
        }

        public async Task<FilmsResponse<FilmPersonInfo>> GetPersonByNameAsync(string name, CancellationToken ct = default, int page = 1)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["name"] = name,
                ["page"] = page.ToString()
            };
            string path = $"{constants.PersonsUrlV1}";
            return await GetResponseDataAsync<FilmsResponse<FilmPersonInfo>>(path, ct, queryParams);
        }

        public async Task<PersonResponse> GetStaffByPersonIdAsync(int personId, CancellationToken ct = default)
        {
            string path = $"{constants.StaffUrlV1}/{personId}";
            return await GetResponseDataAsync<PersonResponse>(path, ct);
        }
    }
}
