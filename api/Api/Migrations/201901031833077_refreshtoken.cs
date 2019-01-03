namespace resource_server.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refreshtoken : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Audiences");
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Audiences", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Audiences", "RefreshTokenLifeTime", c => c.Int(nullable: false));
            AddColumn("dbo.Audiences", "AllowedOrigin", c => c.String(maxLength: 100));
            AlterColumn("dbo.Audiences", "ClientId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Audiences", "Base64Secret", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Audiences", "ClientId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Audiences");
            AlterColumn("dbo.Audiences", "Base64Secret", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Audiences", "ClientId", c => c.String(nullable: false, maxLength: 32));
            DropColumn("dbo.Audiences", "AllowedOrigin");
            DropColumn("dbo.Audiences", "RefreshTokenLifeTime");
            DropColumn("dbo.Audiences", "Active");
            DropTable("dbo.RefreshTokens");
            AddPrimaryKey("dbo.Audiences", "ClientId");
        }
    }
}
