using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIs.Models
{
    public class MonetizationInfo
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
