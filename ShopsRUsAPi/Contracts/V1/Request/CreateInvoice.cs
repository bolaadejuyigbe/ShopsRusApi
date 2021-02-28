using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Request
{
    public class CreateInvoice
    {
        public long Id { get; set; }

        public long customerId { get; set; }

        public List<CreateOrder> createOrders { get; set; }


    }
}
