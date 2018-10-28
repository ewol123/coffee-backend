using Api.Models;
using coffee.Api.Controllers;
using System;
using System.Collections.Generic;
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

            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

            /*var user = new ApplicationUser()
            {
                UserName = createUserModel.Email,
                Email = createUserModel.Email,
                Level = 3,
                JoinDate = DateTime.Now.Date,
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }*/


            return Created("header", headers); 

        }


    }
}
