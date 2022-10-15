﻿using Newtonsoft.Json;

namespace MovieAPIs.UnofficialKinopoiskApi.Models
{
    public class Country
    {
        public int Id { get; set; }

        [JsonProperty("country")]
        public string Name { get; set; }
    }
}
