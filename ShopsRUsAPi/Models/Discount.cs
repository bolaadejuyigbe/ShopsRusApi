using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Models
{
    public class Discount
    {
        [Key]
        [Required]
        public long DiscountId { get; set; }
        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Percentage { get; set; }



    }
}
