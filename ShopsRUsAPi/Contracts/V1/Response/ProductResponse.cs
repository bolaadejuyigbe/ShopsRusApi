using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Response
{
    public class ProductResponse
    {

        public string Name { get; set; }

        public decimal Amount { get; set; }


        public bool IsGroccery { get; set; }
    }
}
