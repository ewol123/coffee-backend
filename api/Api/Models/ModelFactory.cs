﻿using coffee.Api.Entities;
using coffee.Api.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace coffee.Api.Models
{
    //thiss class defines what we return to the requests
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Level = appUser.Level,
                JoinDate = appUser.JoinDate,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }

        public RoleReturnModel Create(IdentityRole appRole)
        {

            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }

        public List<OrderReturnModel> Create(List<OrderedProduct> orderedProduct) {

            List<OrderReturnModel> OrderedProductList = new List<OrderReturnModel>();

            foreach (var op in orderedProduct) {

                var price = int.Parse(op.Coffee.Price);
                var totalPrice = op.Quantity * price;

                OrderedProductList.Add(new OrderReturnModel
                {
                    Id = op.OrderedProductId,
                    Name = op.Coffee.Name,
                    ImagePath = op.Coffee.ImagePath,
                    Price = op.Coffee.Price,
                    Description = op.Coffee.Description,
                    Strength = op.Coffee.Strength,
                    Quantity = op.Quantity,
                    TotalPrice = totalPrice,
                    CoffeeId = op.Coffee.CoffeeId
                });

            }

            return OrderedProductList;

        }

        public CoffeeReturnModel Create(Coffee coffee) {

            return new CoffeeReturnModel
            {
                Id = coffee.CoffeeId,
                Name = coffee.Name,
                ImagePath = coffee.ImagePath,
                Price = coffee.Price,
                Description = coffee.Description,
                Strength = coffee.Strength
            };

        }


    }

        public class UserReturnModel
        {
            public string Url { get; set; }
            public string Id { get; set; }
            public string UserName { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; }
            public int Level { get; set; }
            public DateTime JoinDate { get; set; }
            public IList<string> Roles { get; set; }
            public IList<System.Security.Claims.Claim> Claims { get; set; }
        }


        public class RoleReturnModel
        {
            public string Url { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class OrderReturnModel
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public int CoffeeId { get; set; }
    }

        public class CoffeeReturnModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }

    }

  



}