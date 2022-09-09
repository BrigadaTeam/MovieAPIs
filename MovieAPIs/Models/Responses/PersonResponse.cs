namespace MovieAPIs.Models
{
    public class PersonResponse
    {
        public int? PersonId { get; set; }
        public string WebUrl { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string Sex { get; set; }
        public string PosterUrl { get; set; }
        public int? Growth { get; set; }
        public string Birthday { get; set; }
        public string Death { get; set; }
        public int? Age { get; set; }
        public string BirthPlace { get; set; }
        public string DeathPlace { get; set; }
        public string Profession { get; set; }
        public int HasAwards { get; set; }
        public string[] Facts { get; set; }
        public Spouse[] Spouses { get; set; }
        public FilmPersonInfoById[] Films { get; set; }
    }
}

