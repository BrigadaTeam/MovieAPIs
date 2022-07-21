namespace MovieAPIs.Models
{
    public class DigitalRelease
    {
        public int FilmId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public int Year { get; set; }
        public string PosterUrl { get; set; }
        public string PosterUrlPreview { get; set; }
        public Country[] Countries { get; set; }
        public Genre[] Genres { get; set; }
        public float Rating { get; set; }
        public int RatingVoteCount { get; set; }
        public float ExpectationsRating { get; set; }
        public int ExpectationsRatingVoteCount { get; set; }
        public int Duration { get; set; }
        public string ReleaseDate { get; set; }
    }
}
