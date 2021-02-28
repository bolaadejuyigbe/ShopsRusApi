using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Models
{
    public class Invoice
    {
        [Key]
        [Required]
        public long Id { get; set; }
   
        public long discountId { get; set; }

        [ForeignKey(nameof(discountId))]
        public Discount discount { get; set; }

    
        public long CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer customer { get; set; }
      

        public List<Items> Items { get; set; }
        [Required]
        public decimal billDiscountPercentage { get; set; }
        [Required]
        public decimal totalDiscountAmount { get; set; }
        [Required]
        public decimal totalproductAmount { get; set; }
        [Required]
        public decimal totalAmount { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }

        internal void updateTotalwithdiscount()
        {
            totalAmount = (totalproductAmount - totalDiscountAmount);
        }

    }
}
