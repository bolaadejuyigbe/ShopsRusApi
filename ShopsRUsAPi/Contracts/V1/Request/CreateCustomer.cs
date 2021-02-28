using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Request
{
    public class CreateCustomer
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string MobileNum { get; set; }  
        public DateTime Datecreated { get; set; }
        public bool isAffiliate { get; set; }
        public bool isEmployee { get; set; }
    }
}
