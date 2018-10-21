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
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/americano.jpg?alt=media&token=80bbab09-ef4c-4c5e-b6b0-d0289c81373c",
                Price = "3",
                Description = "Americano is a type of coffee drink prepared by diluting an espresso with hot water, giving it a similar strength to, but different flavor from traditionally brewed coffee. The strength of an Americano varies with the number of shots of espresso and the amount of water added."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = americano, Ingredient = coffeePowder, amount = 19 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = americano, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = americano, Amount = 120, Material = "hot water", Unit = "ml" });

            var bonbón = new Coffee
            {
                Name = "Bonbón",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/bonb%C3%B3n.jpg?alt=media&token=798528b1-0ce5-4d85-8964-06cd45f2e496",
                Price = "4",
                Description = "Cafe Bonbón was made popular in Valencia, Spain, and spread gradually to the rest of the country. It uses espresso served with sweetened condensed milk in a 1:1 ratio."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = bonbón, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = bonbón, Ingredient = condensedMilk, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = bonbón, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = bonbón, Amount = 30, Material = "condensed milk", Unit = "ml" });

            var borgia = new Coffee
            {
                Name = "Borgia",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/borgia.jpg?alt=media&token=ffa247b7-9f66-4c32-9da5-81beb38a7d08",
                Price = "6",
                Description = "A cafe borgia is a mocha with orange rind and sometimes orange flavoring added. Often served with whipped cream and topped with cinnamon."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = borgia, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = borgia, Ingredient = chocolate, amount = 30 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = borgia, Ingredient = whippedCream, amount = 30 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = borgia, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = borgia, Amount = 120, Material = "hot chocolate", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = borgia, Amount = 30, Material = "whipped cream", Unit = "ml" });

            var auLait = new Coffee
            {
                Name = "Café au lait",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/cafeaulait.jpg?alt=media&token=3529a9c7-bc6b-4c0b-8825-60dd80699206",
                Price = "6",
                Description = "Café au lait is coffee with hot milk added. It differs from white coffee, which is coffee with cold milk or other whitener added. "
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = auLait, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = auLait, Ingredient = milk, amount = 90 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = auLait, Amount = 90, Material = "french press coffee", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = auLait, Amount = 90, Material = "scalded milk", Unit = "ml" });

            var affogato = new Coffee
            {
                Name = "Café affogato",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/caf%C3%A9affogato.jpg?alt=media&token=53c1b831-a24a-4869-85ae-a4e5b40b63e1",
                Price = "6",
                Description = "An affogato (Italian for 'drowned') is an Italian coffee-based dessert. It usually takes the form of a scoop of vanilla gelato or ice cream topped or 'drowned' with a shot of hot espresso."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = affogato, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = affogato, Ingredient = iceCream, amount = 90 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = affogato, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = affogato, Amount = 90, Material = "ice cream", Unit = "ml" });


            var conhielo = new Coffee
            {
                Name = "Café con hielo",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/caf%C3%A9conhielo.jpg?alt=media&token=b1bf4659-9093-49bb-bb83-428aeeab7968",
                Price = "4",
                Description = " Con hielo (Iced Coffee) is a type of coffee served cold, brewed various brewing methods, with the fundamental division being cold brew."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = conhielo, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = conhielo, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = conhielo, Amount = 3, Material = "ice cubes", Unit = "piece" });


            var conleche = new Coffee
            {
                Name = "Café con leche",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/caf%C3%A9conleche.jpg?alt=media&token=8ff5f70b-d5f5-4765-b0aa-f76f20d183a7",
                Price = "5",
                Description = "Café con leche (Spanish: 'coffee with milk') is a Spanish coffee beverage consisting of strong and bold coffee (usually espresso) mixed with scalded milk in approximately a 1:1 ratio."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = conleche, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = conleche, Ingredient = milk, amount = 90 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = conleche, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = conleche, Amount = 90, Material = "hot milk", Unit = "ml" });

            var crema = new Coffee
            {
                Name = "Café crema",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/caf%C3%A9crema.jpg?alt=media&token=ae4e34c5-03fd-4733-8637-be11593e893d",
                Price = "3",
                Description = "A long espresso drink primarily served in Germany, Switzerland and Austria and northern Italy."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = crema, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = crema, Amount = 150, Material = "much longer brewed espresso", Unit = "ml" });



            var cubano = new Coffee
            {
                Name = "Café cubano",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/caf%C3%A9cubano.jpg?alt=media&token=10087cfa-71f9-49a3-ba6f-ce8a125a0316",
                Price = "4",
                Description = "Café cubano is a type of espresso that originated in Cuba. Specifically, it refers to an espresso shot which is sweetened with demerara sugar which has been whipped with the first and strongest drops of espresso."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cubano, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cubano, Amount = 30, Material = "espresso", Unit = "ml" });

            var tiempo = new Coffee
            {
                Name = "Café del tiempo",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/caf%C3%A9deltiempo.jpg?alt=media&token=88ab9abc-0e45-41fa-a797-ef3ad9f0bbad",
                Price = "4",
                Description = "The coffee, an espresso or café solo, is served in one cup freshly brewed. On the side is a glass of ice, spiked with a slice of lemon. The coffee is poured over the ice by the customer before drinking."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = tiempo, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = tiempo, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = tiempo, Amount = 3, Material = "ice cubes", Unit = "piece" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = tiempo, Amount = 1, Material = "lemon slice", Unit = "piece" });



            var caphesuada = new Coffee
            {
                Name = "Ca phe sua da",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/caphesuada.jpg?alt=media&token=83ff4177-5732-4d15-9c86-3e59cd2711a2",
                Price = "7",
                Description = "Ca phe sua da, is Vietnamese iced coffee with sweetened condensed milk. This is done by filling up the coffee cup with 2-3 tablespoons or more of sweetened condensed milk prior to the drip filter process."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = caphesuada, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = caphesuada, Ingredient = condensedMilk, amount = 15 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = caphesuada, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = caphesuada, Amount = 6, Material = "ice cubes", Unit = "piece" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = caphesuada, Amount = 15, Material = "softened condensed milk", Unit = "ml" });

            var cappucino = new Coffee
            {
                Name = "Cappucino",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/cappucino.jpg?alt=media&token=c16fb741-9fc2-40c4-addf-def9f042b59d",
                Price = "7",
                Description = "Cappucino is an espresso-based coffee drink that originated in Italy, and is traditionally prepared with double espresso and steamed milk foam."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cappucino, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cappucino, Ingredient = milk, amount = 120 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cappucino, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cappucino, Amount = 60, Material = "steamed milk", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cappucino, Amount = 60, Material = "milk foam", Unit = "ml" });

            var chailatte = new Coffee
            {
                Name = "Chai latte",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/chailatte.jpg?alt=media&token=90ca8ab1-48ed-4f45-9dcc-4fe5a8001fce",
                Price = "4",
                Description = "Black tea infused with cinnamon, clove, and other warming spices is combined with steamed milk and topped with foam for the perfect balance of sweet and spicy."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = chailatte, Ingredient = milk, amount = 90 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = chailatte, Amount = 90, Material = "spiced black tea", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = chailatte, Amount = 90, Material = "steamed milk", Unit = "ml" });

            var cortadito = new Coffee
            {
                Name = "Cortadito",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/cortadito.jpg?alt=media&token=05f74f31-5c1c-46ee-bd71-1b351f29c5f8",
                Price = "3",
                Description = "Strong coffee beverage served hot at Cuban cafés, often sweetened. Similar to Italian espresso."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortadito, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortadito, Ingredient = milk, amount = 30 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cortadito, Amount = 30, Material = "cubano", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cortadito, Amount = 30, Material = "warm milk", Unit = "ml" });

            var cortado = new Coffee
            {
                Name = "Cortado",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/cortado.jpg?alt=media&token=7ea1655c-4c57-4e54-8265-669f01820bdd",
                Price = "3",
                Description = "A cortado is a Spanish beverage consisting of espresso mixed with a roughly equal amount of warm milk to reduce the acidity. The milk in a cortado is steamed, but not frothy and 'texturized' as in many Italian coffee drinks."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortado, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = cortado, Ingredient = milk, amount = 30 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cortado, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = cortado, Amount = 30, Material = "warm milk", Unit = "ml" });

            var doppio = new Coffee
            {
                Name = "Doppio",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/doppio.jpg?alt=media&token=93869985-11ae-4256-bf83-92114d3c1922",
                Price = "4",
                Description = "Doppio espresso is a double shot, extracted using a double coffee filter in the portafilter."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = doppio, Ingredient = coffeePowder, amount = 19 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = doppio, Amount = 60, Material = "espresso", Unit = "ml" });

            var espressino = new Coffee
            {
                Name = "Espressino",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/espressino.jpg?alt=media&token=eb484454-cccb-4a6d-8b2c-f25d9e54002e",
                Price = "5",
                Description = "Espressino (not espresso) is an Italian coffee drink made from equal parts espresso, with some cocoa powder on the bottom of the cup and on top of the drink, and a part of milk as well. "
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressino, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressino, Ingredient = milk, amount = 30 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressino, Ingredient = cocoaPowder, amount = 2 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = espressino, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = espressino, Amount = 30, Material = "steamed milk", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = espressino, Amount = 2, Material = "cocoa powder", Unit = "g" });

            var espresso = new Coffee
            {
                Name = "Espresso",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/espresso.jpg?alt=media&token=9d455345-38b2-472a-a367-c8f3d4124303",
                Price = "3",
                Description = "Espresso is coffee brewed by expressing or forcing a small amount of nearly boiling water under pressure through finely ground coffee beans. "
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espresso, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = espresso, Amount = 30, Material = "espresso", Unit = "ml" });

            var espressoromano = new Coffee
            {
                Name = "Espresso romano",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/espressoromano.jpg?alt=media&token=43672538-073b-406e-84cf-0776a6c02364",
                Price = "3",
                Description = "Espresso Romano (Roman espresso) is espresso with a twist of lemon zest and a little lemon juice."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = espressoromano, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = espressoromano, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = espressoromano, Amount = 1, Material = "lemon slice", Unit = "piece" });


            var flatwhite = new Coffee
            {
                Name = "Flat white",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/flatwhite.jpg?alt=media&token=eccb722c-94f7-491f-8c0d-2a316ff24137",
                Price = "4",
                Description = "A flat white is a coffee drink consisting of espresso with microfoam (steamed milk with small, fine bubbles and a glossy or velvety consistency)."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = flatwhite, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = flatwhite, Ingredient = milk, amount = 120 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = flatwhite, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = flatwhite, Amount = 120, Material = "steamed milk", Unit = "ml" });


            var irishcoffee = new Coffee
            {
                Name = "Irish coffee",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/irish%20coffee.jpg?alt=media&token=c11b2a62-8824-40d2-b241-786f420d62a2",
                Price = "7",
                Description = "Irish coffee is a cocktail consisting of hot coffee, Irish whiskey, and sugar. Some recipes specify that brown sugar should be used, stirred, and topped with thick cream."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = irishcoffee, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = irishcoffee, Ingredient = irishWhiskey, amount = 60 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = irishcoffee, Amount = 120, Material = "french press coffee", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = irishcoffee, Amount = 60, Material = "irish whiskey", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = irishcoffee, Amount = 75, Material = "heavy cream", Unit = "ml" });


            var latte = new Coffee
            {
                Name = "Latte",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/latte.jpg?alt=media&token=975faa19-15e0-4fcf-b40d-1d5b333a409e",
                Price = "7",
                Description = "Our dark, rich espresso balanced with steamed milk and a light layer of foam. A perfect milk forward warm up."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = latte, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = latte, Ingredient = milk, amount = 180 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = latte, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = latte, Amount = 180, Material = "steamed milk", Unit = "ml" });

            var longblack = new Coffee
            {
                Name = "Long black",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/longblack.jpg?alt=media&token=b25ba912-9f6a-4c19-b162-f55ef4ab87eb",
                Price = "4",
                Description = "A long black is made by pouring a double-shot of espresso or ristretto over hot water. Usually the water is also heated by the espresso machine."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = longblack, Ingredient = coffeePowder, amount = 19 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = longblack, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = longblack, Amount = 120, Material = "hot water", Unit = "ml" });

            var lungo = new Coffee
            {
                Name = "Lungo",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/lungo.jpg?alt=media&token=d5408a82-e674-4f33-947a-8d1df1acb02d",
                Price = "4",
                Description = "Lungo is a coffee beverage made by using an espresso machine to make an Italian-style coffee with much more water, resulting in a larger coffee."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = lungo, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = lungo, Amount = 30, Material = "longer brewed espresso", Unit = "ml" });

            var macchiato = new Coffee
            {
                Name = "Macchiato",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/macchiato.jpg?alt=media&token=7be4e8da-929e-4964-acfa-d2734006d708",
                Price = "4",
                Description = "Caffe macchiato, sometimes called espresso macchiato, is an espresso coffee drink with a small amount of milk, usually foamed."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = macchiato, Ingredient = coffeePowder, amount = 8 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = macchiato, Ingredient = milk, amount = 10 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = macchiato, Amount = 30, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = macchiato, Amount = 10, Material = "milk foam", Unit = "ml" });

            var mocha = new Coffee
            {
                Name = "Mocha",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/mocha.jpg?alt=media&token=3827725a-e2b5-4634-b6cf-16f76c63bdd2",
                Price = "5",
                Description = "A caffe mocha, also called mocaccino, is a chocolate-flavored variant of a caffe latte"
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = mocha, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = mocha, Ingredient = chocolate, amount = 120 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = mocha, Ingredient = whippedCream, amount = 30 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = mocha, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = mocha, Amount = 120, Material = "hot chocolate", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = mocha, Amount = 30, Material = "whipped cream", Unit = "ml" });

            var ristretto = new Coffee
            {
                Name = "Ristretto",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/ristretto.jpg?alt=media&token=57a9a921-ad26-4fcf-9a46-776497dfe801",
                Price = "3",
                Description = "Ristretto is traditionally a short shot of espresso coffee made with the normal amount of ground coffee but extracted with about half the amount of water in the same amount of time by using a finer grind."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = ristretto, Ingredient = coffeePowder, amount = 8 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = ristretto, Amount = 20, Material = "espresso", Unit = "ml" });


            var turkishcoffee = new Coffee
            {
                Name = "Turkish coffee",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/turkishcoffee.jpg?alt=media&token=5218d923-93ac-41aa-818a-70a5e84a62d9",
                Price = "4",
                Description = "Turkish coffee is made by bringing very finely ground coffee beans with water and usually sugar to the boil in a special pot called cezve. As soon as the mixture begins to froth, and before it boils over, the coffee is distributed to individual cups."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = turkishcoffee, Ingredient = coffeePowder, amount = 10 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = turkishcoffee, Amount = 10, Material = "ground coffee", Unit = "g" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = turkishcoffee, Amount = 180, Material = "sugar water", Unit = "ml" });

            var vienna = new Coffee
            {
                Name = "Vienna",
                ImagePath = "https://firebasestorage.googleapis.com/v0/b/cofeedeville.appspot.com/o/vienna.jpeg?alt=media&token=dfea3f2c-4998-43a6-923e-04d5312cb93b",
                Price = "4",
                Description = "Vienna is the name of a popular traditional cream-based coffee beverage. It is made by preparing two shots of strong black espresso in a standard sized coffee cup and infusing the coffee with whipped cream."
            };

            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = vienna, Ingredient = coffeePowder, amount = 19 });
            context.IngredientCoffees.AddOrUpdate(new IngredientCoffees { Coffee = vienna, Ingredient = whippedCream, amount = 30 });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = vienna, Amount = 60, Material = "espresso", Unit = "ml" });
            context.ProductCompositions.AddOrUpdate(new ProductComposition { Coffee = vienna, Amount = 30, Material = "whipped cream", Unit = "ml" });

            context.SaveChanges();








        }




    }
}
