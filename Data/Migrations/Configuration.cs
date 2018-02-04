using System.Linq;
using System.Data.Entity.Migrations;
using MunicipalityTaxes.Core.Domain.Classifications;
using MunicipalityTaxes.Core.Domain.Taxes;
using System;

namespace MunicipalityTaxes.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MunicipalityTaxes.Data.DataAccess>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(DataAccess context)
        {
            var vilnius = context.Municipalities.Where(q => q.Name == "Vilnius").FirstOrDefault();

            if (vilnius == null)
            {
                vilnius = new Municipality() { Name = "Vilnius" };
                context.Municipalities.Add(vilnius);
                context.SaveChanges();
            }

            var taxCount = context.Taxes.Count();

            if (taxCount == 0)
            {
                context.Taxes.Add(Tax.Factory.Create(vilnius, TaxType.Yearly, new DateTime(2016, 01, 01), new DateTime(2016, 12, 31), 0.2M));
                context.Taxes.Add(Tax.Factory.Create(vilnius, TaxType.Monthly, new DateTime(2016, 05, 01), new DateTime(2016, 05, 31), 0.4M));
                context.Taxes.Add(Tax.Factory.CreateDaily(vilnius, new DateTime(2016, 01, 01), 0.1M));
                context.Taxes.Add(Tax.Factory.CreateDaily(vilnius, new DateTime(2016, 12, 25), 0.1M));
                context.SaveChanges();
            }
        }
    }
}
