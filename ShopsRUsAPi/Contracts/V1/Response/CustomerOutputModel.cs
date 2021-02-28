using ShopsRUsAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Response
{
    public class CustomerOutputModel
    {
        public PagingHeader Paging { get; set; }
        public List<CustomerResponse> customerResponses { get; set; }
      
    }
}
