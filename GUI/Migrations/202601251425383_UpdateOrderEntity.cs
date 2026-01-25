namespace PriceComp.GUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "StoreID", "dbo.Stores");
            DropIndex("dbo.Products", new[] { "StoreID" });
            DropColumn("dbo.Products", "StoreID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "StoreID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "StoreID");
            AddForeignKey("dbo.Products", "StoreID", "dbo.Stores", "StoreID", cascadeDelete: true);
        }
    }
}
