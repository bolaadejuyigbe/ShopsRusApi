using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Models
{
    public class Items
    {
        [Key]
        [Required]
        public long Id { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public decimal itemAmount { get; set; }

        [ForeignKey("products")]
        public long productId { get; set; }
        public Product products { get; set; }
        [ForeignKey("invoices")]
        public long invoicesId { get; set; }
        public Invoice invoices { get; set; }
        [Required]
        public bool isGrocery { get; set; }
    }
}
