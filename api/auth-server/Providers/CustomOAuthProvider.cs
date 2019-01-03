using auth_server.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace auth_server.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;
            Audience audience = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }
        
             var db = ApplicationDbContext.Create();
             audience = db.Audiences.Find(context.ClientId);
            if (audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (!audience.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", audience.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", audience.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            
            IdentityUser user = await userManager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", $"The user name: {context.UserName} or password: {context.Password} is incorrect.");
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "User did not confirm email.");
                return;
            }
            ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
           
            oAuthIdentity.AddClaim(new Claim("userId", user.Id));
            oAuthIdentity.AddClaim(new Claim("client_id", context.ClientId));


            string staffAudienceId = ConfigurationManager.AppSettings["as:staffAudienceId"];
            if (context.ClientId == staffAudienceId)
            {
                 bool isAdmin = await userManager.IsInRoleAsync(user.Id,"SuperAdmin");
                 bool isStaffMember = await userManager.IsInRoleAsync(user.Id, "Staff");

                if (!isStaffMember && !isAdmin)
                {
                    context.SetError("invalid_role","User is not a staff member.");
                    return;
                }
            }   

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                     {
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            
            context.Validated(ticket);
            
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

        
            context.Validated();

            return Task.FromResult<object>(null);
        }
    }
}