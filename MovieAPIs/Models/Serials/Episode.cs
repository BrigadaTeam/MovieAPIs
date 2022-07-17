namespace MovieAPIs.Models.Serials
{
    public class Episode
    {
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string Synopsis { get; set; }
        public string ReleaseDate { get; set; }
    }
}
