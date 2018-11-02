using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class OrderBindingModel
    {
       
        [Required]
        public string TableNum { get; set; }

        [Required]
        public string Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }
      
    }
}