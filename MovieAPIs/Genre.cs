using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieAPIs
{
    public class Genre
    {
        [JsonPropertyName("genre")]
        public string Name { get; set; }
    }
}
