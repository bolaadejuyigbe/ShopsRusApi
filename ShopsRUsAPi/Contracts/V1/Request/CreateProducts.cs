using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Request
{
    public class CreateProducts
    {
        public long Id { get; set; }

        public string Name { get; set; }
   
        public decimal Amount { get; set; }

   
        public bool IsGroccery { get; set; }
    }
}
