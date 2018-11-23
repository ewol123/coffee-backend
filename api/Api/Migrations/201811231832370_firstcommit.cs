namespace coffee.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstcommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audiences",
                c => new
                    {
                        ClientId = c.String(nullable: false, maxLength: 32),
                        Base64Secret = c.String(nullable: false, maxLength: 80),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.Coffees",
                c => new
                    {
                        CoffeeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ImagePath = c.String(nullable: false, maxLength: 400),
                        Price = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 400),
                        Strength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CoffeeId);
            
            CreateTable(
                "dbo.IngredientCoffees",
                c => new
                    {
                        Coffee_Id = c.Int(nullable: false),
                        Ingredient_Id = c.Int(nullable: false),
                        amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Coffee_Id, t.Ingredient_Id })
                .ForeignKey("dbo.Coffees", t => t.Coffee_Id, cascadeDelete: true)
                .ForeignKey("dbo.Ingredients", t => t.Ingredient_Id, cascadeDelete: true)
                .Index(t => t.Coffee_Id)
                .Index(t => t.Ingredient_Id);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Amount = c.Int(nullable: false),
                        Unit = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.IngredientId);
            
            CreateTable(
                "dbo.OrderedProducts",
                c => new
                    {
                        OrderedProductId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        CoffeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderedProductId)
                .ForeignKey("dbo.Coffees", t => t.CoffeeId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId)
                .Index(t => t.CoffeeId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        PaymentMethod = c.String(nullable: false, maxLength: 80),
                        TotalPrice = c.Int(nullable: false),
                        Status = c.String(nullable: false, maxLength: 40),
                        TableNum = c.Int(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        Payed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Users", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Level = c.Byte(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.FavoriteCoffees",
                c => new
                    {
                        CoffeeId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CoffeeId, t.UserId })
                .ForeignKey("dbo.Coffees", t => t.CoffeeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CoffeeId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.FavoriteCoffees", "UserId", "dbo.Users");
            DropForeignKey("dbo.FavoriteCoffees", "CoffeeId", "dbo.Coffees");
            DropForeignKey("dbo.OrderedProducts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "ApplicationUserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.OrderedProducts", "CoffeeId", "dbo.Coffees");
            DropForeignKey("dbo.IngredientCoffees", "Ingredient_Id", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientCoffees", "Coffee_Id", "dbo.Coffees");
            DropIndex("dbo.FavoriteCoffees", new[] { "UserId" });
            DropIndex("dbo.FavoriteCoffees", new[] { "CoffeeId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Orders", new[] { "ApplicationUserId" });
            DropIndex("dbo.OrderedProducts", new[] { "CoffeeId" });
            DropIndex("dbo.OrderedProducts", new[] { "OrderId" });
            DropIndex("dbo.IngredientCoffees", new[] { "Ingredient_Id" });
            DropIndex("dbo.IngredientCoffees", new[] { "Coffee_Id" });
            DropTable("dbo.FavoriteCoffees");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderedProducts");
            DropTable("dbo.Ingredients");
            DropTable("dbo.IngredientCoffees");
            DropTable("dbo.Coffees");
            DropTable("dbo.Audiences");
        }
    }
}
