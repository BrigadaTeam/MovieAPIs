using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.ResponseModels
{
    public class GenresAndCountriesSearchResponse
    {
        public Genre[] Genres { get; set; }
        public Country[] Countries { get; set; }
    }
}
