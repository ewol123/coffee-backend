using resource_server.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace resource_server.Api.Entities
{
    public class Coffee
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CoffeeId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(400)]
        [Required]
        public string ImagePath { get; set; }

        [MaxLength(100)]
        [Required]
        public string Price { get; set; }

        [MaxLength(400)]
        [Required]
        public string Description { get; set; }

        [Required]
        public int Strength { get; set; }


        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}