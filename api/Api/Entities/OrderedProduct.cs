using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace coffee.Api.Entities
{
    public class OrderedProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderedProductId { get; set; }


        [Required]
        public int Quantity { get; set; }


        [Required]
        public int OrderId { get; set; }


        public virtual Order Order { get; set; }

        [Required]
        public int CoffeeId { get; set; }

        public virtual Coffee Coffee { get; set; }


    }
}