using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace coffee.Api.Entities
{
    public class ProductComposition
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompositionId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Material { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public int CoffeeId { get; set; }

        public virtual Coffee Coffee { get; set; }



    }
}