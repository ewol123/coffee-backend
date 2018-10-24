using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace coffee.Api.Models
{
    public class PaginationBindingModel
    {
        [Required]
        [Display(Name = "Page")]
        public string Page { get; set; }


        [Required]
        [Display(Name = "ItemsPerPage")]
        public string ItemsPerPage { get; set; }

        [Required]
        [Display(Name ="Query")]
        public string Query { get; set; }

    }
}