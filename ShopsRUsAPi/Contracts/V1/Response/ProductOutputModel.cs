using ShopsRUsAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts.V1.Response
{
    public class ProductOutputModel
    {
        public PagingHeader Paging { get; set; }
        public List<ProductResponse> ProductResponses { get; set; }
    }
}
