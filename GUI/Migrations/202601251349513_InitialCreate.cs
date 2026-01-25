namespace PriceComp.GUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Street = c.String(),
                        HouseNumber = c.String(),
                        ApartmentNumber = c.String(),
                        EncryptedCardNumber = c.String(),
                    })
                .PrimaryKey(t => t.ClientID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        OfferID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Offer_OfferID = c.Int(),
                    })
                .PrimaryKey(t => new { t.OrderID, t.OfferID })
                .ForeignKey("dbo.Offers", t => t.Offer_OfferID)
                .ForeignKey("dbo.Offers", t => t.OfferID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.OfferID)
                .Index(t => t.Offer_OfferID);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        OfferID = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PromoPrice = c.Decimal(precision: 18, scale: 2),
                        PromoDescription = c.String(),
                        Product_ProductID = c.Int(),
                        Store_StoreID = c.Int(),
                    })
                .PrimaryKey(t => t.OfferID)
                .ForeignKey("dbo.Products", t => t.Product_ProductID)
                .ForeignKey("dbo.Stores", t => t.Store_StoreID)
                .Index(t => t.Product_ProductID)
                .Index(t => t.Store_StoreID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        StoreID = c.Int(nullable: false),
                        Name = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitName = c.String(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Stores", t => t.StoreID, cascadeDelete: true)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        StoreID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DeliveryCost = c.Decimal(precision: 18, scale: 2),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.StoreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "OfferID", "dbo.Offers");
            DropForeignKey("dbo.Offers", "Store_StoreID", "dbo.Stores");
            DropForeignKey("dbo.Offers", "Product_ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "StoreID", "dbo.Stores");
            DropForeignKey("dbo.OrderDetails", "Offer_OfferID", "dbo.Offers");
            DropForeignKey("dbo.Orders", "ClientID", "dbo.Clients");
            DropIndex("dbo.Products", new[] { "StoreID" });
            DropIndex("dbo.Offers", new[] { "Store_StoreID" });
            DropIndex("dbo.Offers", new[] { "Product_ProductID" });
            DropIndex("dbo.OrderDetails", new[] { "Offer_OfferID" });
            DropIndex("dbo.OrderDetails", new[] { "OfferID" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "ClientID" });
            DropTable("dbo.Stores");
            DropTable("dbo.Products");
            DropTable("dbo.Offers");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.Clients");
        }
    }
}
