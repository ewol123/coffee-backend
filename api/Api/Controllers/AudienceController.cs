using resource_server.Api.Entities;
using resource_server.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace resource_server.Api.Controllers
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
               Audience newAudience = AudiencesStore.AddAudience(audienceModel.Name);

                return Ok(newAudience);

            }
        }
}