
using auth_server.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace auth_server.Providers
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "as:client_id";

        private readonly string _issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string id = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidOperationException("AuthenticationTicket.Properties does not include audience");

            var db = ApplicationDbContext.Create();
            Audience audience = db.Audiences.Find(id);

            if (audience == null) throw new InvalidOperationException("Audience not found");

            string audienceId = audience.ClientId;


            string symmetricKeyAsBase64 = audience.Base64Secret;


            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyByteArray);

            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256Signature);


            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

      

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}