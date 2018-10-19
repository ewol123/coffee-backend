using coffee.Api.Entities;
using coffee.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace coffee.Api.Controllers
{
    //testing purposes only, don't use it in production
    [RoutePrefix("api/audience")]
        //make this class child of 'BaseApiController' so you can use the database
        public class AudienceController : BaseApiController
    {
            [Route("")]
            public IHttpActionResult Post(AudienceModel audienceModel)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //TODO: store audience in a database
               Audience newAudience = AudiencesStore.AddAudience(audienceModel.Name);

                return Ok(newAudience);

            }
        }
}