namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class FilmPersonInfoById
    {
        public int FilmId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public double? Rating { get; set; }
        public bool General { get; set; }
        public string Description { get; set; }
        public string ProfessionKey { get; set; }
    }
}
