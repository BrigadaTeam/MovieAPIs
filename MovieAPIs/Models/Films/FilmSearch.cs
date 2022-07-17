
namespace MovieAPIs.Models
{
    public class FilmSearch
    {
        public int FilmId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string FilmLength { get; set; }
        public Country[] Countries { get; set; }
        public Genre[] Genres { get; set; }
        public string Raring { get; set; }
        public int RatingVoteCount { get; set; }
        public string PosterUrl { get; set; }
        public string PosterUrlPreview { get; set; }
    }
}
