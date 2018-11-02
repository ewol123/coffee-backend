using Api.Models;
using coffee.Api.Controllers;
using coffee.Api.Entities;
using coffee.Api.Infrastructure;
using coffee.Api.Models;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrderController : BaseApiController
    {


        [Authorize(Roles = "User, Admin")]
        [Route("create")]
        public async Task<IHttpActionResult> CreateOrder(OrderBindingModel orderBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var db = ApplicationDbContext.Create();

            try
            {
                var user = await db.Users.Include("Orders").FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null)
                {
                    return BadRequest("user not found");
                }


                var tableNum = int.Parse(orderBindingModel.TableNum);
                var quantity = int.Parse(orderBindingModel.Quantity);
                var productId = int.Parse(orderBindingModel.ProductId);

                var coffee = await db.Coffees.FirstOrDefaultAsync(c => c.CoffeeId == productId);


                if (coffee == null) {
                    return BadRequest("coffee not found");
                }

                var order = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && !o.Status.Equals("inactive"));

                if (order == null)
                {

                    var coffeePrice = int.Parse(coffee.Price);
                    var price = coffeePrice  * quantity;

                     order = new Order
                    {
                        ApplicationUser = user,
                        PaymentMethod = "online",
                        TotalPrice = price,
                        Status = "draft",
                        TableNum = tableNum,
                        Payed = false

                    };

                    db.Orders.Add(order);

                    var orderedProduct = new OrderedProduct
                    {
                     Coffee = coffee,
                     Order = order,
                     Quantity = quantity
                    };

                    db.OrderedProducts.Add(orderedProduct);


                    var saveChangesResult = await db.SaveChangesAsync();

                    if (saveChangesResult == 0) 
                    {
                        return BadRequest("cannot be created");
                    }


                    return Created("ok","created");
                }


                return BadRequest("order already exists");
            }
            catch (Exception e ) {
                return BadRequest(e.ToString());
            }

        }

        [Authorize(Roles = "User, Admin")]
        [Route("")]
        public async Task<IHttpActionResult> GetOrders(OrderBindingModel orderBindingModel)
        {

            var db = ApplicationDbContext.Create();

            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null)
                {
                    return NotFound();
                }

                var order = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && !o.Status.Equals("inactive"));


                if (order == null) {
                    return NotFound();
                }

                var orderedProducts = await db.OrderedProducts.Include("Coffee").Where(op => op.OrderId == order.OrderId).ToListAsync();

                List<OrderReturnModel> orderReturnModels = new List<OrderReturnModel>();

                foreach (var op in orderedProducts) {

                    orderReturnModels.Add(TheModelFactory.Create(op));
                    
                }

                int allPrice = 0;
                foreach (var orders in orderReturnModels) {
                    allPrice += orders.TotalPrice;
                }

                var orderList = new OrdersReturnModel {
                 AllPrice = allPrice,
                 orders = orderReturnModels
                };



                return Ok(orderList);
             }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }

        }

        [Authorize(Roles = "User, Admin")]
        [Route("increaseProduct")]
        [HttpPost]
        public async Task<IHttpActionResult> IncreaseProduct(OrderBindingModel orderBindingModel)
        {
            try
            {

                var db = ApplicationDbContext.Create();

                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null)
                {
                    return NotFound();
                }


                var quantity = int.Parse(orderBindingModel.Quantity);
                var productId = int.Parse(orderBindingModel.ProductId);

                var coffee = await db.Coffees.FirstOrDefaultAsync(c => c.CoffeeId == productId);


                if (coffee == null)
                {
                    return NotFound();
                }

                var order = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && !o.Status.Equals("inactive"));

                if (order == null) {
                    return BadRequest();
                }

                var orderedProduct = await db.OrderedProducts.Where(op => op.OrderId == order.OrderId && op.CoffeeId == coffee.CoffeeId).FirstOrDefaultAsync();

                if (orderedProduct == null) {

                    orderedProduct = new OrderedProduct
                    {
                        Coffee = coffee,
                        Order = order,
                        Quantity = quantity
                    };

                    db.OrderedProducts.Add(orderedProduct);


                    var saveChangesResult = await db.SaveChangesAsync();

                    if (saveChangesResult == 0)
                    {
                        return BadRequest("cannot be created");
                    }
                    return Created("ok", "product added");
                }

                orderedProduct.Quantity += quantity;
                var updateResult = await db.SaveChangesAsync();

                if (updateResult == 0)
                {
                    return BadRequest("cannot be updated");
                }

                return Ok("updated");

            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }

        }


        [Authorize(Roles = "User, Admin")]
        [Route("decreaseProduct")]
        [HttpPost]
        public async Task<IHttpActionResult> decreaseProduct(OrderBindingModel orderBindingModel)
        {
            try
            {

                var db = ApplicationDbContext.Create();

                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null)
                {
                    return NotFound();
                }


                var quantity = int.Parse(orderBindingModel.Quantity);
                var productId = int.Parse(orderBindingModel.ProductId);

                var coffee = await db.Coffees.FirstOrDefaultAsync(c => c.CoffeeId == productId);


                if (coffee == null)
                {
                    return NotFound();
                }

                var order = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && !o.Status.Equals("inactive"));

                if (order == null)
                {
                    return BadRequest();
                }

                var orderedProduct = await db.OrderedProducts.Where(op => op.OrderId == order.OrderId && op.CoffeeId == coffee.CoffeeId).FirstOrDefaultAsync();

                if (orderedProduct == null)
                {
                    return BadRequest();
                }

                if (orderedProduct.Quantity == 1) {
                    db.OrderedProducts.Remove(orderedProduct);

                    var deleteResult = await db.SaveChangesAsync();

                    if (deleteResult == 0)
                    {
                        return BadRequest("cannot be deleted");
                    }

                    return Ok("deleted");

                }

                orderedProduct.Quantity -= quantity;
                var updateResult = await db.SaveChangesAsync();

                if (updateResult == 0)
                {
                    return BadRequest("cannot be updated");
                }

                return Ok("decreased");

            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }

        }


    }
}
