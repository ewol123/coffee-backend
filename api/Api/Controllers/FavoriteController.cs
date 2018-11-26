using coffee.Api.Controllers;
using coffee.Api.Entities;
using coffee.Api.Infrastructure;
using coffee.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/favorite")]
    public class FavoriteController : BaseApiController
    {
        [Authorize(Roles = "User, Admin")]
        [Route("addFavorite")]
        [HttpPost]
        public async Task<IHttpActionResult> AddFavorite(int id)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null) return BadRequest();

                var coffee = await db.Coffees.FirstOrDefaultAsync(c => c.CoffeeId.Equals(id));

                if (coffee == null) return BadRequest();


                user.Coffees = new List<Coffee>();
                       user.Coffees.Add(coffee);

                var result = await db.SaveChangesAsync();

                if (result == 0) return BadRequest("cannot be created");

                return Ok();
            }
            catch (Exception e )
            {

                return BadRequest(e.ToString());
            }

        }

        [Authorize(Roles = "User, Admin")]
        [Route("deleteFavorite")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteFavorite(int id)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var user = await db.Users.Include(x => x.Coffees).FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));

                if (user == null) return BadRequest();

                var coffee = user.Coffees.Where(x => x.CoffeeId.Equals(id)).First();

                user.Coffees.Remove(coffee);

                var result = await db.SaveChangesAsync();

                if (result == 0) return BadRequest("cannot be deleted");

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }

        }

       


        [Authorize(Roles = "User, Admin")]
        [Route("getFavorites")]
        public async Task<IHttpActionResult> getFavorites()
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var user = await db.Users.Include(x => x.Coffees).FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name));


                if (user == null) return BadRequest();


                List<CoffeeReturnModel> coffeeList = new List<CoffeeReturnModel>();

                var coffees = user.Coffees;

                foreach(var coffee in coffees)
                {
                    coffeeList.Add(TheModelFactory.Create(coffee));
                }


                return Ok(coffeeList);

            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }

        }

    }
}
