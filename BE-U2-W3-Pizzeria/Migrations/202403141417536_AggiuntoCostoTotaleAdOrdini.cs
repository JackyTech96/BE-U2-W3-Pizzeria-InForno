namespace BE_U2_W3_Pizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCostoTotaleAdOrdini : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ordini", "CostoTotale", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ordini", "CostoTotale");
        }
    }
}
