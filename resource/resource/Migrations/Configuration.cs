namespace resource_server.Api.Migrations
{
    using resource_server.Api.Entities;
    using resource_server.Api.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<resource_server.Api.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(resource_server.Api.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var admin = new ApplicationUser()
            {
                UserName = "admin@coffeeshop.com",
                Email = "admin@coffeeshop.com",
                EmailConfirmed = true,
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(admin, "Admin123!");




            var staff = new ApplicationUser()
            {
                UserName = "staff@coffeeshop.com",
                Email = "staff@coffeeshop.com",
                EmailConfirmed = true,
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(staff, "Staff123!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
                roleManager.Create(new IdentityRole { Name = "Staff" });
            }

            var adminUser = manager.FindByName("admin@coffeeshop.com");
            var staffUser = manager.FindByName("staff@coffeeshop.com");
            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin", "User" });

            manager.AddToRoles(staffUser.Id, new string[] { "Staff" });
            var user = new ApplicationUser()
            {
                UserName = "user@coffeeshop.com",
                Email = "user@coffeeshop.com",
                EmailConfirmed = true,
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)

            };

            manager.Create(user, "Hello123!");

            var normalUser = manager.FindByName("user@coffeeshop.com");

            manager.AddToRoles(normalUser.Id, new string[] { "User" });




            //create initial ingredients
            context.Ingredients.AddOrUpdate(ingr => ingr.Name,
            new Ingredient { Name = "coffee powder", Amount = 10000, Unit = "g" },
            new Ingredient { Name = "cocoa powder", Amount = 10000, Unit = "g" },
            new Ingredient { Name = "milk", Amount = 10000, Unit = "ml" },
            new Ingredient { Name = "ice cream", Amount = 10000, Unit = "ml" },
            new Ingredient { Name = "condensed milk", Amount = 10000, Unit = "ml" },
            new Ingredient { Name = "chocolate", Amount = 10000, Unit = "g" },
            new Ingredient { Name = "irish whiskey", Amount = 10000, Unit = "ml" },
            new Ingredient { Name = "lemon juice", Amount = 10000, Unit = "ml" },
            new Ingredient { Name = "whipped cream", Amount = 10000, Unit = "ml" }
            );
            context.SaveChanges();



            var coffeePowder = context.Ingredients.Where(i => i.Name == "coffee powder")
                            .SingleOrDefault();

            var cocoaPowder = context.Ingredients.Where(i => i.Name == "cocoa powder")
                            .SingleOrDefault();

            var milk = context.Ingredients.Where(i => i.Name == "milk")
                            .SingleOrDefault();

            var iceCream = context.Ingredients.Where(i => i.Name == "ice cream")
                            .SingleOrDefault();

            var condensedMilk = context.Ingredients.Where(i => i.Name == "condensed milk")
                            .SingleOrDefault();

            var chocolate = context.Ingredients.Where(i => i.Name == "chocolate")
                            .SingleOrDefault();

            var irishWhiskey = context.Ingredients.Where(i => i.Name == "irish whiskey")
                            .SingleOrDefault();

            var lemonJuice = context.Ingredients.Where(i => i.Name == "lemon juice")
                            .SingleOrDefault();

            var whippedCream = context.Ingredients.Where(i => i.Name == "whipped cream")
                            .SingleOrDefault();


            //create initial coffees
            var americano = new Coffee
            {
                Name = "Americano",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540298834/coffee/americano.jpg",
                Price = "3",
                Strength = 3,
                Description = "Americano is a type of coffee drink prepared by diluting an espresso with hot water, giving it a similar strength to, but different flavor from traditionally brewed coffee. The strength of an Americano varies with the number of shots of espresso and the amount of water added."

            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = americano, Ingredient = coffeePowder, amount = 19 });

            var bonbón = new Coffee
            {
                Name = "Bonbón",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299732/coffee/bonb%C3%B3n.jpg",
                Price = "4",
                Strength = 2,
                Description = "Cafe Bonbón was made popular in Valencia, Spain, and spread gradually to the rest of the country. It uses espresso served with sweetened condensed milk in a 1:1 ratio."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = bonbón, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = bonbón, Ingredient = condensedMilk, amount = 8 });

            var borgia = new Coffee
            {
                Name = "Borgia",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299732/coffee/borgia.jpg",
                Price = "6",
                Strength = 2,
                Description = "A cafe borgia is a mocha with orange rind and sometimes orange flavoring added. Often served with whipped cream and topped with cinnamon."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = borgia, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = borgia, Ingredient = chocolate, amount = 30 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = borgia, Ingredient = whippedCream, amount = 30 });

            var auLait = new Coffee
            {
                Name = "Café au lait",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/cafeaulait.jpg",
                Price = "6",
                Strength = 3,
                Description = "Café au lait is coffee with hot milk added. It differs from white coffee, which is coffee with cold milk or other whitener added. "
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = auLait, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = auLait, Ingredient = milk, amount = 90 });

            var affogato = new Coffee
            {
                Name = "Café affogato",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299732/coffee/caf%C3%A9affogato.jpg",
                Price = "6",
                Strength = 2,
                Description = "An affogato (Italian for 'drowned') is an Italian coffee-based dessert. It usually takes the form of a scoop of vanilla gelato or ice cream topped or 'drowned' with a shot of hot espresso."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = affogato, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = affogato, Ingredient = iceCream, amount = 90 });


            var conhielo = new Coffee
            {
                Name = "Café con hielo",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299732/coffee/caf%C3%A9conhielo.jpg",
                Price = "4",
                Strength = 3,
                Description = " Con hielo (Iced Coffee) is a type of coffee served cold, brewed various brewing methods, with the fundamental division being cold brew."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = conhielo, Ingredient = coffeePowder, amount = 8 });


            var conleche = new Coffee
            {
                Name = "Café con leche",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/caf%C3%A9conleche.jpg",
                Price = "5",
                Strength = 4,
                Description = "Café con leche (Spanish: 'coffee with milk') is a Spanish coffee beverage consisting of strong and bold coffee (usually espresso) mixed with scalded milk in approximately a 1:1 ratio."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = conleche, Ingredient = coffeePowder, amount = 8 });

            var crema = new Coffee
            {
                Name = "Café crema",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/caf%C3%A9crema.jpg",
                Price = "3",
                Strength = 4,
                Description = "A long espresso drink primarily served in Germany, Switzerland and Austria and northern Italy."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = crema, Ingredient = coffeePowder, amount = 8 });



            var cubano = new Coffee
            {
                Name = "Café cubano",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/caf%C3%A9cubano.jpg",
                Price = "4",
                Strength = 5,
                Description = "Café cubano is a type of espresso that originated in Cuba. Specifically, it refers to an espresso shot which is sweetened with demerara sugar which has been whipped with the first and strongest drops of espresso."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cubano, Ingredient = coffeePowder, amount = 8 });

            var tiempo = new Coffee
            {
                Name = "Café del tiempo",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/caf%C3%A9deltiempo.jpg",
                Price = "4",
                Strength = 3,
                Description = "The coffee, an espresso or café solo, is served in one cup freshly brewed. On the side is a glass of ice, spiked with a slice of lemon. The coffee is poured over the ice by the customer before drinking."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = tiempo, Ingredient = coffeePowder, amount = 8 });


            var caphesuada = new Coffee
            {
                Name = "Ca phe sua da",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/caphesuada.jpg",
                Price = "7",
                Strength = 2,
                Description = "Ca phe sua da, is Vietnamese iced coffee with sweetened condensed milk. This is done by filling up the coffee cup with 2-3 tablespoons or more of sweetened condensed milk prior to the drip filter process."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = caphesuada, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = caphesuada, Ingredient = condensedMilk, amount = 15 });

            var cappucino = new Coffee
            {
                Name = "Cappucino",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/cappucino.jpg",
                Price = "7",
                Strength = 2,
                Description = "Cappucino is an espresso-based coffee drink that originated in Italy, and is traditionally prepared with double espresso and steamed milk foam."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cappucino, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cappucino, Ingredient = milk, amount = 120 });

            var chailatte = new Coffee
            {
                Name = "Chai latte",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/chailatte.jpg",
                Price = "4",
                Strength = 2,
                Description = "Black tea infused with cinnamon, clove, and other warming spices is combined with steamed milk and topped with foam for the perfect balance of sweet and spicy."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = chailatte, Ingredient = milk, amount = 90 });

            var cortadito = new Coffee
            {
                Name = "Cortadito",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299733/coffee/cortadito.jpg",
                Price = "3",
                Strength = 5,
                Description = "Strong coffee beverage served hot at Cuban caf�s, often sweetened. Similar to Italian espresso."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortadito, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortadito, Ingredient = milk, amount = 30 });

            var cortado = new Coffee
            {
                Name = "Cortado",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299734/coffee/cortado.jpg",
                Price = "3",
                Strength = 3,
                Description = "A cortado is a Spanish beverage consisting of espresso mixed with a roughly equal amount of warm milk to reduce the acidity. The milk in a cortado is steamed, but not frothy and 'texturized' as in many Italian coffee drinks."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortado, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortado, Ingredient = milk, amount = 30 });

            var doppio = new Coffee
            {
                Name = "Doppio",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299734/coffee/doppio.jpg",
                Price = "4",
                Strength = 5,
                Description = "Doppio espresso is a double shot, extracted using a double coffee filter in the portafilter."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = doppio, Ingredient = coffeePowder, amount = 19 });

            var espressino = new Coffee
            {
                Name = "Espressino",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299734/coffee/espressino.jpg",
                Price = "5",
                Strength = 4,
                Description = "Espressino (not espresso) is an Italian coffee drink made from equal parts espresso, with some cocoa powder on the bottom of the cup and on top of the drink, and a part of milk as well. "
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressino, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressino, Ingredient = milk, amount = 30 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressino, Ingredient = cocoaPowder, amount = 2 });

            var espresso = new Coffee
            {
                Name = "Espresso",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299734/coffee/espresso.jpg",
                Price = "3",
                Strength = 4,
                Description = "Espresso is coffee brewed by expressing or forcing a small amount of nearly boiling water under pressure through finely ground coffee beans. "
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espresso, Ingredient = coffeePowder, amount = 8 });

            var espressoromano = new Coffee
            {
                Name = "Espresso romano",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299734/coffee/espressoromano.jpg",
                Price = "3",
                Strength = 3,
                Description = "Espresso Romano (Roman espresso) is espresso with a twist of lemon zest and a little lemon juice."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressoromano, Ingredient = coffeePowder, amount = 8 });


            var flatwhite = new Coffee
            {
                Name = "Flat white",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299734/coffee/flatwhite.jpg",
                Price = "4",
                Strength = 2,
                Description = "A flat white is a coffee drink consisting of espresso with microfoam (steamed milk with small, fine bubbles and a glossy or velvety consistency)."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = flatwhite, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = flatwhite, Ingredient = milk, amount = 120 });


            var irishcoffee = new Coffee
            {
                Name = "Irish coffee",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299735/coffee/irish_coffee.jpg",
                Price = "7",
                Strength = 3,
                Description = "Irish coffee is a cocktail consisting of hot coffee, Irish whiskey, and sugar. Some recipes specify that brown sugar should be used, stirred, and topped with thick cream."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = irishcoffee, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = irishcoffee, Ingredient = irishWhiskey, amount = 60 });


            var latte = new Coffee
            {
                Name = "Latte",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299736/coffee/latte.jpg",
                Price = "7",
                Strength = 2,
                Description = "Our dark, rich espresso balanced with steamed milk and a light layer of foam. A perfect milk forward warm up."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = latte, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = latte, Ingredient = milk, amount = 180 });

            var longblack = new Coffee
            {
                Name = "Long black",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299736/coffee/longblack.jpg",
                Price = "4",
                Strength = 3,
                Description = "A long black is made by pouring a double-shot of espresso or ristretto over hot water. Usually the water is also heated by the espresso machine."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = longblack, Ingredient = coffeePowder, amount = 19 });

            var lungo = new Coffee
            {
                Name = "Lungo",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299736/coffee/lungo.jpg",
                Price = "4",
                Strength = 4,
                Description = "Lungo is a coffee beverage made by using an espresso machine to make an Italian-style coffee with much more water, resulting in a larger coffee."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = lungo, Ingredient = coffeePowder, amount = 8 });

            var macchiato = new Coffee
            {
                Name = "Macchiato",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299736/coffee/macchiato.jpg",
                Price = "4",
                Strength = 2,
                Description = "Caffe macchiato, sometimes called espresso macchiato, is an espresso coffee drink with a small amount of milk, usually foamed."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = macchiato, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = macchiato, Ingredient = milk, amount = 10 });

            var mocha = new Coffee
            {
                Name = "Mocha",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299737/coffee/mocha.jpg",
                Price = "5",
                Strength = 1,
                Description = "A caffe mocha, also called mocaccino, is a chocolate-flavored variant of a caffe latte"
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = mocha, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = mocha, Ingredient = chocolate, amount = 120 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = mocha, Ingredient = whippedCream, amount = 30 });

            var ristretto = new Coffee
            {
                Name = "Ristretto",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299737/coffee/ristretto.jpg",
                Price = "3",
                Strength = 1,
                Description = "Ristretto is traditionally a short shot of espresso coffee made with the normal amount of ground coffee but extracted with about half the amount of water in the same amount of time by using a finer grind."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = ristretto, Ingredient = coffeePowder, amount = 8 });


            var turkishcoffee = new Coffee
            {
                Name = "Turkish coffee",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299737/coffee/turkishcoffee.jpg",
                Price = "4",
                Strength = 1,
                Description = "Turkish coffee is made by bringing very finely ground coffee beans with water and usually sugar to the boil in a special pot called cezve. As soon as the mixture begins to froth, and before it boils over, the coffee is distributed to individual cups."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = turkishcoffee, Ingredient = coffeePowder, amount = 10 });

            var vienna = new Coffee
            {
                Name = "Vienna",
                ImagePath = "https://res.cloudinary.com/dctmxbu9q/image/upload/v1540299738/coffee/vienna.jpg",
                Price = "4",
                Strength = 2,
                Description = "Vienna is the name of a popular traditional cream-based coffee beverage. It is made by preparing two shots of strong black espresso in a standard sized coffee cup and infusing the coffee with whipped cream."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = vienna, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = vienna, Ingredient = whippedCream, amount = 30 });

            context.SaveChanges();








        }




    }
}
