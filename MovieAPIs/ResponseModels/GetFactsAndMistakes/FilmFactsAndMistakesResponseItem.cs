using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPIs.Utils;

namespace MovieAPIs.ResponseModels
{
    public class FilmFactsAndMistakesResponseItem
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public bool Spoiler { get; set; }
    }
}
