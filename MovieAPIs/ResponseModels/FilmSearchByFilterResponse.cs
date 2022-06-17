using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.ResponseModels
{
    public class FilmSearchByFilterResponse
    {
        public int TotalPages { get; set; }
        public Film[] Items { get; set; }
    }
}
