using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Response
{
    public class DiscountResponse
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public decimal Percentage { get; set; }
    }
}
