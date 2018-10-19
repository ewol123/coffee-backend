using coffee.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace coffee.Api.Controllers
{
    [RoutePrefix("api/coffees")]
    public class CoffeeController : BaseApiController
    {



        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("pagination")]
        public IHttpActionResult GetCoffees(PaginationBindingModel paginationBindingModel )
        {
            //formula: page -1 * itemsPerPage = eredmény   limit five

            int page = int.Parse(paginationBindingModel.Page);
            int itemsPerPage = int.Parse(paginationBindingModel.ItemsPerPage);
            int skipAmount = (page - 1) * itemsPerPage;

            var coffees =  applicationDbContext.Coffees
                          .OrderBy(c=> c.CoffeeId)
                          .Skip(skipAmount)
                          .Take(itemsPerPage);

            if (coffees != null) {

                return Ok(coffees);
            }
                          

            return NotFound();


        }


    }
}
