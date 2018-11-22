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
        [Route("")]
        public async Task<IHttpActionResult> GetOrders()
        {

            try
            {
                var db = ApplicationDbContext.Create();

                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null) return NotFound();

                var order = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && o.Status.Equals("draft"));


                if (order == null) return NotFound();


                var orderedProducts = await db.OrderedProducts.Include("Coffee").Where(op => op.OrderId == order.OrderId).ToListAsync();

                return Ok(TheModelFactory.Create(orderedProducts));
            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }

        }

        [Authorize(Roles = "User, Admin")]
        [Route("order/{coffeeId}")]
        public async Task<IHttpActionResult> GetCoffee(int coffeeId)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var coffee = await db.Coffees.FirstOrDefaultAsync(c => c.CoffeeId == coffeeId);

                return Ok(TheModelFactory.Create(coffee));
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

                if (user == null) return NotFound();

                var tableNum = int.Parse(orderBindingModel.TableNum);
                var quantity = int.Parse(orderBindingModel.Quantity);
                var productId = int.Parse(orderBindingModel.ProductId);

                var coffee = await db.Coffees.FirstOrDefaultAsync(c => c.CoffeeId == productId);


                if (coffee == null) return NotFound();

                var order = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && o.Status.Equals("draft"));

                if (order == null)
                {

                    var coffeePrice = int.Parse(coffee.Price);
                    var price = coffeePrice * quantity;

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

                    var newOrderedProduct = new OrderedProduct
                    {
                        Coffee = coffee,
                        Order = order,
                        Quantity = quantity
                    };

                    db.OrderedProducts.Add(newOrderedProduct);


                    var saveChangesResult = await db.SaveChangesAsync();

                    if (saveChangesResult == 0) return BadRequest("cannot be created");

                    return Created("ok", "created");
                }

                var orderedProduct = await db.OrderedProducts.Where(op => op.OrderId == order.OrderId && op.CoffeeId == coffee.CoffeeId).FirstOrDefaultAsync();

                if (orderedProduct == null)
                {

                    orderedProduct = new OrderedProduct
                    {
                        Coffee = coffee,
                        Order = order,
                        Quantity = quantity
                    };

                    db.OrderedProducts.Add(orderedProduct);


                    var saveChangesResult = await db.SaveChangesAsync();

                    if (saveChangesResult == 0) return BadRequest("cannot be created");

                    return Created("ok", "product added");
                }

                orderedProduct.Quantity += quantity;
                var updateResult = await db.SaveChangesAsync();

                if (updateResult == 0) return BadRequest("cannot be updated");



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
        public async Task<IHttpActionResult> DecreaseProduct(OrderBindingModel orderBindingModel)
        {
            try
            {

                var db = ApplicationDbContext.Create();

                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null) return NotFound();

                var quantity = int.Parse(orderBindingModel.Quantity);
                var productId = int.Parse(orderBindingModel.ProductId);

                var coffee = await db.Coffees.FirstOrDefaultAsync(c => c.CoffeeId == productId);

                if (coffee == null) return NotFound();

                var order = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && o.Status.Equals("draft"));

                if (order == null) return BadRequest();


                var orderedProduct = await db.OrderedProducts.FirstOrDefaultAsync(op => op.OrderId == order.OrderId && op.CoffeeId == coffee.CoffeeId);

                if (orderedProduct == null) return BadRequest();


                if (orderedProduct.Quantity == 1)
                {
                    db.OrderedProducts.Remove(orderedProduct);

                    var deleteResult = await db.SaveChangesAsync();

                    if (deleteResult == 0) return BadRequest("cannot be deleted");

                    var checkOrderedProducts = await db.OrderedProducts.Where(op => op.OrderId == order.OrderId).ToListAsync();

                    if (checkOrderedProducts.Count < 1)
                    {
                        order.Status = "inactive";
                        var updateStatus = await db.SaveChangesAsync();

                        if (updateStatus == 0) return BadRequest("cannot set to inactive");
                        return Ok("deleted");
                    }
                    return Ok("deleted");
                }

                orderedProduct.Quantity -= quantity;
                var updateResult = await db.SaveChangesAsync();

                if (updateResult == 0) return BadRequest("cannot be updated");




                return Ok("decreased");

            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

        }

        [Authorize(Roles = "User, Admin")]
        [Route("deleteProduct")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null) return NotFound();

                var orderedProduct = await db.OrderedProducts.FirstOrDefaultAsync(op => op.OrderedProductId == id);

                if (orderedProduct == null) return BadRequest();

                db.OrderedProducts.Remove(orderedProduct);

                var deleteResult = await db.SaveChangesAsync();

                if (deleteResult == 0) return BadRequest("cannot be deleted");


                var checkOrder = await db.Orders.FirstOrDefaultAsync(o => o.ApplicationUserId.Equals(user.Id) && o.Status.Equals("draft"));
                var checkOrderedProducts = await db.OrderedProducts.Where(op => op.OrderId == checkOrder.OrderId).ToListAsync();

                if (checkOrderedProducts.Count < 1)
                {
                    checkOrder.Status = "inactive";
                    var updateStatus = await db.SaveChangesAsync();

                    if (updateStatus == 0) return BadRequest("cannot set to inactive");

                }

                return Ok("deleted");


            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }
        }



        [Authorize(Roles = "User, Admin")]
        [Route("updateOrder")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateOrder(string paymentMethod)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var user = await db.Users
                    .Include("Orders")
                    .Include(i => i.Orders.Select(o => o.OrderedProducts))
                    .Include(i => i.Orders.Select(o => o.OrderedProducts.Select(op => op.Coffee))).FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null) return NotFound();

                var order = user.Orders.Where(o => o.Status.Equals("draft")).FirstOrDefault();
                order.PaymentMethod = paymentMethod;

                order.Payed = paymentMethod.Equals("online") ? true : false;

                order.Status = "placed";

                var orderedProducts = order.OrderedProducts;

                var totalPrice = 0;
                foreach (var product in orderedProducts) {

                    totalPrice += int.Parse(product.Coffee.Price) * product.Quantity;

                }

                order.TotalPrice = totalPrice;

                var updateStatus = await db.SaveChangesAsync();

                if (updateStatus == 0) return BadRequest("Cannot complete order");

                return Ok("Order Placed");
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
                
            }


        }
    }

}
