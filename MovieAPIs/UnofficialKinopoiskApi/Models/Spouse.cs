namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class Spouse
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public bool Divorced { get; set; }
        public string DivorcedReason { get; set; }
        public string Sex { get; set; }
        public int Children { get; set; }
        public string WebUrl { get; set; }
        public string Relation { get; set; }
    }
}

