namespace MovieAPIs.Models
{
    public class FilmRelease : FilmSearch
    {
        public int ExpectationsRatingVoteCount { get; set; }
        public int Duration { get; set; }
        public string ReleaseDate { get; set; }
        public string ExpectationsRating { get; set; }
    }
}
