
namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class RelatedFilm
    {
        public int FilmId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameOriginal { get; set; }
        public string PosterUrl { get; set; }
        public string PosterUrlPreview { get; set; }
        public string RelationType { get; set; }
    }
}
