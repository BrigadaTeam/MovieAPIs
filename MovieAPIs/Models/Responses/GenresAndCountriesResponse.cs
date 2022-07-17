using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.Models
{
    public class GenresAndCountriesResponse
    {
        public Genre[] Genres { get; set; }
        public Country[] Countries { get; set; }
    }
}
