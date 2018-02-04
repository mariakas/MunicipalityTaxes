using System.Data.Entity.Migrations;

namespace MunicipalityTaxes.Data.Migrations
{
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Municipality",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Tax",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaxType = c.Byte(nullable: false),
                        MunicipalityId = c.Int(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Municipality", t => t.MunicipalityId)
                .Index(t => t.MunicipalityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tax", "MunicipalityId", "dbo.Municipality");
            DropIndex("dbo.Tax", new[] { "MunicipalityId" });
            DropIndex("dbo.Municipality", new[] { "Name" });
            DropTable("dbo.Tax");
            DropTable("dbo.Municipality");
        }
    }
}
