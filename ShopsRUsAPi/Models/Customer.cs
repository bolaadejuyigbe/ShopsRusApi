using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Models
{
    public class Customer
    {
        [Key]
        public long CustomerId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(20)]
        public string MobileNum { get; set; }
        [Required]
        public DateTime Datecreated { get; set; }
        [Required]
        public bool isAffiliate { get; set; }
        public bool isEmployee { get; set; }


    }
}
