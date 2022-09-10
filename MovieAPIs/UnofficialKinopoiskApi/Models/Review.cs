namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class Review
    {
        public int KinopoiskId { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public int PositiveRating { get; set; }
        public int NegativeRating { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
