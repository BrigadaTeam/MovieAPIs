
namespace MovieAPIs.Models
{
    public class FilmDistributionsResponseItems
    {
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Date { get; set; }
        public bool? ReRelease { get; set; }
        public Country Country { get; set; }
        public Company[] Companies { get; set; }

    }
}
