namespace coffee.Api.Migrations
{
    using coffee.Api.Entities;
    using coffee.Api.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<coffee.Api.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(coffee.Api.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));



            var user = new ApplicationUser()
            {
                UserName = "gypeti23@gmail.com",
                Email = "gypeti23@gmail.com",
                EmailConfirmed = true,
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(user, "Admin123!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("gypeti23@gmail.com");

            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });

            //create initial ingredients
            context.Ingredients.AddOrUpdate(ingr => ingr.Name,
            new Ingredient {Name = "coffee powder", Amount = 10000,  Unit = "g" },
            new Ingredient {Name = "cocoa powder", Amount = 10000, Unit = "g" },             
            new Ingredient{Name = "milk", Amount = 10000, Unit = "ml"},
            new Ingredient { Name = "ice cream", Amount = 10000, Unit = "ml" },
            new Ingredient { Name ="condensed milk", Amount = 10000, Unit="ml" },
            new Ingredient {  Name = "chocolate", Amount = 10000, Unit = "g" },
            new Ingredient { Name = "irish whiskey", Amount = 10000, Unit = "ml" },
            new Ingredient  {  Name = "lemon juice",   Amount = 10000, Unit = "ml" });
            context.SaveChanges();

          

            //create initial coffees
            var americano = new Coffee {
                Name = "Americano",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/americano.jpg?alt=media&token=80bbab09-ef4c-4c5e-b6b0-d0289c81373c",
                Price ="5",
                Description = "Americano is a type of coffee drink prepared by diluting an espresso with hot water, giving it a similar strength to, but different flavor from traditionally brewed coffee. The strength of an Americano varies with the number of shots of espresso and the amount of water added."
                

            };

            var coffee_powder = context.Ingredients.Where(i => i.Name == "coffee powder")
                            .SingleOrDefault();

           

            if (coffee_powder != null) {
                var ingredientCoffees = new IngredientCoffees { Coffee = americano, Ingredient = coffee_powder, amount = 8 };
                context.IngredientCoffees.Add(ingredientCoffees);

                var productComposition = new ProductComposition { Coffee= americano, Amount = 60, Material="espresso", Unit="ml" };
                var productComposition2 = new ProductComposition { Coffee = americano, Amount = 120, Material = "hot water", Unit = "ml" };
                context.ProductCompositions.Add(productComposition);
                context.ProductCompositions.Add(productComposition2);
                context.SaveChanges();

            }

        }




    }
}
