using resource_server.Api.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace resource_server.Api
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
             HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);
            ConfigureWebApi(config);
            ConfigureOAuthTokenConsumption(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.MapSignalR("/signalr", new Microsoft.AspNet.SignalR.HubConfiguration());
        }

        public void ConfigureOAuth(IAppBuilder app)
        {

            //invoke our db context method
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            //invoke our usermanager method
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = "http://localhost:5821";
            string androidAudienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            string staffAudienceId = ConfigurationManager.AppSettings["as:staffAudienceId"];
            byte[] androidSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AndroidSecret"]);
            byte[] staffSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:StaffSecret"]);
            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { androidAudienceId, staffAudienceId },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, androidSecret),
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, staffSecret)
                    }
                });
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.EnableSystemDiagnosticsTracing();
          
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}