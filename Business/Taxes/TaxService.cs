using System;
using System.Linq;
using MunicipalityTaxes.Core;
using MunicipalityTaxes.Core.Domain.Classifications;
using MunicipalityTaxes.Core.Domain.Taxes;
using MunicipalityTaxes.Data;

namespace MunicipalityTaxes.Business.Taxes
{
    public class TaxService : ITaxService
    {
        private readonly DataAccess dac;

        public TaxService(DataAccess dac)
        {
            this.dac = dac;
        }

        public ServiceResult CreateDailyTax(string municipality, DateTime day, decimal amount)
        {
            // TODO: Not working prototype

            var result = new ServiceResult();

            if (amount < 0)
            {
                result.AddError("Amount is less or eqoul to zero");
            }

            // TODO: other validation ... 
            // Possible use FluentValidator for convenience, but then need some preperations around, or change some coding style

            if (result.Success)
            {
                // TODO: For optimization possible cache classifications, rarely changed data
                var mun = dac.Get<Municipality>().Where(q => q.Name == municipality).FirstOrDefault();
                dac.Insert(Tax.Factory.CreateDaily(mun, day, amount));
                dac.SaveChanges();
            }


            return result;
        }

        public decimal? Get(string municipality, DateTime date)
        {
            var tax = dac.Get<Tax>().Where(q => q.Municipality.Name == municipality && q.From <= date && date < q.To).OrderBy(q => q.Amount).FirstOrDefault();
            return tax?.Amount;
        }
    }
}
