using resource_server.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace resource_server.Api.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [MaxLength(80)]
        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        [MaxLength(40)]
        public string Status { get; set; }

        [Required]
        public int TableNum { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public bool Payed { get; set; }

        

        public virtual ApplicationUser ApplicationUser { get; set; }


        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }

    }
}