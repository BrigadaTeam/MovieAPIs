namespace MovieAPIs.Models
{
    public class Nomination
    {
        public string Name { get; set; }
        public bool Win { get; set; }
        public string ImageUrl { get; set; }
        public string NominationName { get; set; }
        public int Year { get; set; }
        public PersonAward[] Persons { get; set; }
    }
}
