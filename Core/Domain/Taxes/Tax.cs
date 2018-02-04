using System;
using MunicipalityTaxes.Core.Domain.Classifications;

namespace MunicipalityTaxes.Core.Domain.Taxes
{
    public class Tax : BaseEntity
    {
        protected Tax()
        {
        }

        public TaxType TaxType { get; protected set; }

        public int MunicipalityId { get; protected set; }

        public DateTime From { get; protected set; }

        public DateTime To { get; protected set; }

        public decimal Amount { get; protected set; }

        public Municipality Municipality { get; protected set; }

        public static class Factory
        {
            // TODO: hide
            public static Tax Create(Municipality municipality, TaxType taxType, DateTime from, DateTime to, decimal amount)
            {
                // TODO: if needed some validation - domain object specific
                var fromShort = new DateTime(from.Year, from.Month, from.Day);
                var toShort = new DateTime(to.Year, to.Month, to.Day);
                var tax = new Tax()
                {
                    MunicipalityId = municipality.Id,
                    TaxType = taxType,
                    From = fromShort,
                    To = toShort.AddDays(1).AddSeconds(-1),
                    Amount = amount
                };

                return tax;
            }

            // TODO: Create dedicated  methods for every type, like this, and then Create hide, make private or smth
            public static Tax CreateDaily(Municipality municipality, DateTime day, decimal amount)
            {
                return Tax.Factory.Create(municipality, TaxType.Daily, day, day, amount);
            }
        }
    }
}
