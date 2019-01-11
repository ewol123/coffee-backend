namespace auth_server.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<auth_server.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(auth_server.Infrastructure.ApplicationDbContext context)
        {

            context.Audiences.AddOrUpdate(aud => aud.ClientId, new Audience
            {
                ClientId = "5bd1d38ccf7a428ab3b963ac8bd1e4de",
                Base64Secret = "8Yf2gwxzD1iL0K7bKciHKDQlKbFKyCYzUc-XfUucsX0",
                Name = "android audience",
                Active = true,
                AllowedOrigin = "*",
                RefreshTokenLifeTime = 14400
            });

            context.Audiences.AddOrUpdate(aud => aud.ClientId, new Audience
            {
                ClientId = "24e5a184d2b1488c8dc97587625260fb",
                Base64Secret = "kA4kAg4SE4ZXSM5Zg4Su9Al5XyYVd1CiWHQ_P0b3eIc",
                Name = "staff audience",
                Active = true,
                AllowedOrigin = "*",
                RefreshTokenLifeTime = 14400
            });
        }
    }
}
