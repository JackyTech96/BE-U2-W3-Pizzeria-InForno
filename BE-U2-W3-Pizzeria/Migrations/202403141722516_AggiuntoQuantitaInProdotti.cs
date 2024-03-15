namespace BE_U2_W3_Pizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoQuantitaInProdotti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prodotti", "Quantita", c => c.Int(nullable: false));
            DropColumn("dbo.Prodotti", "PrezzoIngredienti");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prodotti", "PrezzoIngredienti", c => c.Decimal(precision: 10, scale: 2));
            DropColumn("dbo.Prodotti", "Quantita");
        }
    }
}
