using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coffee.Api.Entities
{
    public class IngredientCoffees
    {



        public int Coffee_Id { get; set; }

        public int Ingredient_Id { get; set; }

        public virtual Coffee Coffee { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public int amount { get; set; }


    }
}