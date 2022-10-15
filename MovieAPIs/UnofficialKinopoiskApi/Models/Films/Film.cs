namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class Film
    {
        public int KinopoiskId { get; set; }
        public string ImdbId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameOriginal { get; set; }
        public string PosterUrl { get; set; }
        public string PosterUrlPreview { get; set; }
        public string CoverUrl { get; set; }
        public string LogoUrl { get; set; }
        public int ReviewsCount { get; set; }
        public double? RatingGoodReview { get; set; }
        public int? RatingGoodReviewVoteCount { get; set; }
        public double? RatingKinopoisk { get; set; }
        public int? RatingKinopoiskVoteCount { get; set; }
        public double? RatingImdb { get; set; }
        public int? RatingImdbVoteCount { get; set; }
        public double? RatingFilmCritics { get; set; }
        public int? RatingFilmCriticsVoteCount { get; set; }
        public double? RatingAwait { get; set; }
        public int? RatingAwaitCount { get; set; }
        public double? RatingRfCritics { get; set; }
        public int? RatingRfCriticsVoteCount { get; set; }
        public string WebUrl { get; set; }
        public int? Year { get; set; }
        public int? FilmLength { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string EditorAnnotation { get; set; }
        public bool IsTicketsAvailable { get; set; }
        public string ProductionStatus { get; set; }
        public string Type { get; set; }
        public string RatingMpaa { get; set; }
        public string RatingAgeLimits { get; set; }
        public bool? HasImax { get; set; }
        public bool? Has3D { get; set; }
        public string LastSync { get; set; }
        public Country[] Countries { get; set; }
        public Genre[] Genres { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public bool? Serial { get; set; }
        public bool? ShortFilm { get; set; }
        public bool? Completed { get; set; }
    }
}
