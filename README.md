<h1 align="center">Movie API</h1>

<p align="center">

<img src="https://badges.frapsoft.com/os/v1/open-source.svg?v=103" >
</p>

C# library for working with movie services api.
An open source project containing methods for working with: 

* [Kinopoisk Api Unofficial](https://kinopoiskapiunofficial.tech/documentation/api/)
* [Kinopoisk API](https://kinopoisk.dev/#api) (in progress)


---

## Get Started

### Install .NET

The library is for .net version 6.0 and C# 10.0.

### Project preparation

Install MovieAPI in Nuget Package Manager.

### Quick start

```cs
using MovieAPIs.UnofficialKinopoiskApi

var client = new UnofficialKinopoiskApiClient("api-key");

// An example of getting a movie by its Id on the movie search
var film = client.GetFilmByIdAsync(5664);

// Get movies from a range of pages
var topFilms = client.GetTopFilmsFromPageRangeAsync(fromPage:3, toPage:5);
var filmsByKeyword = client.GetFilmsByKeywordFromPageRangeAsync("Keyword", new Range(1, 4));

// Get from all pages
var filmsByKeywordFromAllPages = client.GetFilmsByKeywordFromPageRangeAsync("Keyword");
```

## Library description

### Description of methods from UnofficialKinopoiskApiClient

|               Method name                 |                               Description                                                       |
|-------------------------------------------|-------------------------------------------------------------------------------------------------|
| `GetTopFilmsFromPageRangeAsync`           | Get a list of movies from various tops and collections from page range                          |
| `GetDigitalReleasesFromPageRangeAsync`    | Get list of digital releases from page range                                                    |
| `GetFilmsByKeywordFromPageRangeAsync`     | Get list of movies by keywords from page range                                                  | 
| `GetFilmsByFiltersFromPageRangeAsync`     | Get a list of movies by different filters from page range                                       |
| `GetViewerReviewsByIdFromPageRangeAsync`  | Returns a list of viewer reviews from page range                                                |
| `GetImagesByIdFromPageRangeAsync`         | Get movie related images from page range                                                        |
| `GetPersonByNameFromPageRangeAsync`       | Search for actors, directors, etc by name from page range                                       |
| `GetFilmByIdAsync`                        | Get movie data by kinopoisk id                                                                  |
| `GetGenresAndCountriesAsync`              | Returns a list of country and genre IDs that have been sold in /api/v2.2/films                  |
| `GetRelatedFilmsAsync`                    | Get a list of similar movies by kinopoisk id                                                    |
| `GetFilmFactsAndMistakesAsync`            | Get data about facts and errors in movies by kinopoisk id                                       |
| `GetFilmDistributionsAsync`               | Get rental data in different countries by kinopoisk id                                          |
| `GetSequelsAndPrequelsByIdAsync`          | Get sequels and prequels by kinopoisk id                                                        |
| `GetBoxOfficeByIdAsync`                   | Get budget and collection information by kinopoisk id                                           |
| `GetSeasonsDataByIdAsync`                 | Get season data for the series by kinopoisk id                                                  |
| `GetStaffByFilmIdAsync`                   | Get data about actors, directors, etc. by kinopoisk id                                          |
| `GetPremieresListAsync`                   | Get list of film premieres                                                                      |
| `GetTrailersAndTeasersByIdAsync`          | Get trailers, teasers, videos for the film by kinopoisk id                                      |
| `GetAwardsByIdAsync`                      | Get film awards data by kinopoisk film id                                                       |
| `GetStaffByPersonIdAsync`                 | Get information about a specific person by kinopoisk id                                         |
| `GetFilmsByKeywordAsync`                  | Get movie list by keyword                                                                       |
| `GetFilmsByFiltersAsync`                  | Get a list of movies by various filters                                                         |
| `GetTopFilmsAsync`                        | Get movies from various tops and collections                                                    |
| `GetViewerReviewsByIdAsync`               | Returns a list of paginated viewer reviews                                                      |
| `GetImagesByIdAsync`                      | Get images associated with the movie with pagination. Each page contains no more than 20 films  |
| `GetPersonByNameAsync`                    | Search for actors, directors, etc. by name. One page can contain up to 50 items in items        |

---

## Developers

* https://github.com/Maksim-Trolina
* https://github.com/EugenKoulik
