using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Request
{
    public class CreateOrder
    {
        public long productId { get; set; }

        public int quantity { get; set; }
    }
}
