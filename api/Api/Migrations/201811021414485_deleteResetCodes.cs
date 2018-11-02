namespace coffee.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteResetCodes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserResetCodes", "ApplicationUserId", "dbo.Users");
            DropIndex("dbo.UserResetCodes", new[] { "ApplicationUserId" });
            DropTable("dbo.UserResetCodes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserResetCodes", "ApplicationUserId");
            AddForeignKey("dbo.UserResetCodes", "ApplicationUserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
