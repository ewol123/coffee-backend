namespace coffee.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coffeeStrength : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductCompositions", "CoffeeId", "dbo.Coffees");
            DropIndex("dbo.ProductCompositions", new[] { "CoffeeId" });
            AddColumn("dbo.Coffees", "Strength", c => c.Int(nullable: false));
            DropTable("dbo.ProductCompositions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductCompositions",
                c => new
                    {
                        CompositionId = c.Int(nullable: false, identity: true),
                        Material = c.String(nullable: false, maxLength: 100),
                        Amount = c.Int(nullable: false),
                        Unit = c.String(nullable: false),
                        CoffeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompositionId);
            
            DropColumn("dbo.Coffees", "Strength");
            CreateIndex("dbo.ProductCompositions", "CoffeeId");
            AddForeignKey("dbo.ProductCompositions", "CoffeeId", "dbo.Coffees", "CoffeeId");
        }
    }
}
