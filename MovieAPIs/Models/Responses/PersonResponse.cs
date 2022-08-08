namespace MovieAPIs.Models
{
    public class PersonResponse : Person
    {
        public int HasAwards { get; set; }
        public string[] Facts { get; set; }
        public Spouse[] Spouses { get; set; }
        public FilmPersonInfo[] Films { get; set; }
    }
}

