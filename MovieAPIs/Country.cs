using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieAPIs
{
    public class Country
    {
        [JsonPropertyName("country")]
        public string Name { get; set; }
    }
}
