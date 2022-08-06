namespace MovieAPIs.Models
{
    public class FilmPremiere
    {
        public int KinopoiskId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public int Year { get; set; }
        public string PosterUrl { get; set; }
        public string PosterUrlPreview { get; set; }
        public Country[] Countries { get; set; }
        public Genre[] Genres { get; set; }
        public int Duration { get; set; }
        public string PremiereRu { get; set; }
    }
}

