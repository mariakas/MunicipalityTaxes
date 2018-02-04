using System;
using System.Data.Entity.Migrations;

namespace MunicipalityTaxes.Data.Migrations
{    
    public partial class TaxAddedIndexForPerformance : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tax", new[] { "MunicipalityId" });
            CreateIndex("dbo.Tax", new[] { "From", "To", "MunicipalityId" });
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tax", new[] { "From", "To", "MunicipalityId" });
            CreateIndex("dbo.Tax", "MunicipalityId");
        }
    }
}
