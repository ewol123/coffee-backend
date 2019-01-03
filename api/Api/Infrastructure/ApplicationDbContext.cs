﻿using resource_server.Api.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace resource_server.Api.Infrastructure
{
    //IdentityDBContext just like the DBContext class is responsible for communicating with the database. the create method returns a new instance of the class
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Audience> Audiences { get; set; }
        public DbSet<Coffee> Coffees { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientCoffees> IngredientCoffees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }


        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder builder)

        {


            base.OnModelCreating(builder);

       

            //rename default tables names generated by identity
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            builder.Entity<IdentityRole>().ToTable("Roles");

            // set our OrderedProduct associations
            builder.Entity<OrderedProduct>()
             .HasRequired(op => op.Order)
             .WithMany(op => op.OrderedProducts)
             .HasForeignKey(op => op.OrderId)
             .WillCascadeOnDelete(false);


            builder.Entity<OrderedProduct>()
             .HasRequired(c=> c.Coffee)
             .WithMany(c=>c.OrderedProducts)
             .HasForeignKey(c=> c.CoffeeId)
             .WillCascadeOnDelete(false);


            // set our Order associations with User
            builder.Entity<Order>()
            .HasRequired(o => o.ApplicationUser)
            .WithMany(o => o.Orders)
            .HasForeignKey(o => o.ApplicationUserId)
            .WillCascadeOnDelete(false);

            // coffees and ingredients many-to-many association
            builder.Entity<IngredientCoffees>()
                .HasKey(ic => new { ic.Coffee_Id, ic.Ingredient_Id });

            builder.Entity<IngredientCoffees>()
                .HasRequired(ic => ic.Coffee)
                .WithMany(ic => ic.Ingredients)
                .HasForeignKey(ic => ic.Coffee_Id);

            builder.Entity<IngredientCoffees>()
                .HasRequired(ic => ic.Ingredient)
                .WithMany(ic => ic.Coffees)
                .HasForeignKey(ic => ic.Ingredient_Id);
          
            //favorite
            builder.Entity<Coffee>()
              .HasMany<ApplicationUser>(b => b.Users) 
              .WithMany(u => u.Coffees)
              .Map(cs =>
              {
                  cs.MapLeftKey("CoffeeId");
                  cs.MapRightKey("UserId");
                  cs.ToTable("FavoriteCoffees");
              });

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}