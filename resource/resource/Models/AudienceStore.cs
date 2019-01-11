using resource_server.Api.Entities;
using resource_server.Api.Infrastructure;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace resource_server.Api.Models
{


    public  class AudiencesStore
    {
        
       
        static AudiencesStore()
        {
          
        }


        public static  Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            var db = ApplicationDbContext.Create();
          
            Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            db.Audiences.Add(newAudience);
             db.SaveChangesAsync();
            return newAudience;
        }

        public static  Audience FindAudience(string clientId)
        {
            var db = ApplicationDbContext.Create();
            Audience audience =  db.Audiences.Find(clientId);
          
            return audience;
            

        }
    }
}