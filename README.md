<h1 align="center">Movie API</h1>

<p align="center">

<img src="https://badges.frapsoft.com/os/v1/open-source.svg?v=103" >
</p>

C# library for working with movie services api.
An open source project containing methods for working with api of the most popular movie services.
Api documentation - [Kinopoisk Api Unofficial](https://kinopoiskapiunofficial.tech/documentation/api/)

---

## Get Started

### Install .NET

The library is for .net version 6.0 and C# 10.0.

### Project preparation

* Install MovieAPI in Nuget Package Manager or or add .dll dependencies.
* Add configuration.json file to the project directory

### Quick start

```cs
using MovieAPIs.UnofficialKinopoiskApi

var client = new UnofficialKinopoiskApiClient("api-key");

// An example of getting a movie by its Id on the movie search
var response = client.GetFilmByIdAsync(5664);
```

## Library description

### Description of methods from UnofficialKinopoiskApiClient

|               Method name                 |          Description          |
|-------------------------------------------|-------------------------------|
| `GetTopFilmsFromPageRangeAsync`           |
| `GetDigitalReleasesFromPageRangeAsync`    |
| `GetFilmsByKeywordFromPageRangeAsync`     |
| `GetFilmsByFiltersFromPageRangeAsync`     |
| `GetViewerReviewsByIdFromPageRangeAsync`  |
| `GetImagesByIdFromPageRangeAsync`         |
| `GetPersonByNameFromPageRangeAsync`       |
| `GetFilmByIdAsync`                        |
| `GetGenresAndCountriesAsync`              |
| `GetRelatedFilmsAsync`                    |
| `GetFilmFactsAndMistakesAsync`            |
| `GetFilmDistributionsAsync`               |
| `GetSequelsAndPrequelsByIdAsync`          |
| `GetBoxOfficeByIdAsync`                   |
| `GetSeasonsDataByIdAsync`                 |
| `GetStaffByFilmIdAsync`                   |
| `GetPremieresListAsync`                   |
| `GetTrailersAndTeasersByIdAsync`          |
| `GetAwardsByIdAsync`                      |
| `GetStaffByPersonIdAsync`                 |
| `GetFilmsByKeywordAsync`                  |
| `GetFilmsByFiltersAsync`                  |
| `GetTopFilmsAsync`                        |
| `GetViewerReviewsByIdAsync`               |
| `GetImagesByIdAsync`                      |
| `GetPersonByNameAsync`                    |

### Technology stack

* .NET 6.0
* C# 10.0
* [Newtonsoft.json](https://www.newtonsoft.com/json)

---

## Developers

* https://github.com/Maksim-Trolina
* https://github.com/EugenKoulik
