namespace coffee.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserResetCodes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserResetCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserResetCodes", "ApplicationUserId", "dbo.Users");
            DropIndex("dbo.UserResetCodes", new[] { "ApplicationUserId" });
            DropTable("dbo.UserResetCodes");
        }
    }
}
