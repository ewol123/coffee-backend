using resource_server.Api.Infrastructure;
using resource_server.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace resource_server.Api.Controllers
{
    [RoutePrefix("api/coffees")]
    public class CoffeeController : BaseApiController
    {

        [Authorize(Roles = "User, Admin")]
        [Route("pagination")]
        public IHttpActionResult GetCoffees(int page, int itemsPerPage, string query = "")
        {
            int skipAmount = (page - 1) * itemsPerPage;

            var coffees = query == "all"
               ? applicationDbContext.Coffees
                         .OrderBy(c => c.CoffeeId)
                         .Skip(skipAmount)
                         .Take(itemsPerPage)



               : applicationDbContext.Coffees
                         .OrderBy(c => c.CoffeeId)
                         .Where(c => c.Name.Contains(query))
                         .Skip(skipAmount)
                         .Take(itemsPerPage);



            if (coffees != null)
            {

                return Ok(coffees);
            }


            return NotFound();
        }

       
    }
}
