
namespace MovieAPIs.ResponseModels
{
    public class FilmSearchResponse
    {
        public string Keyword { get; set; }
        public int PagesCount { get; set; }
        public int SearchFilmsCountResult { get; set; }
        public FilmSearchResponseFilms[] Films { get; set; }
    }   
}
