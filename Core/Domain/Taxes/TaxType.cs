using System;

namespace MunicipalityTaxes.Core.Domain.Taxes
{
    public enum TaxType : byte
    {
        Yearly = 10,
        Monthly = 20,
        Weekly = 30,
        Daily = 40
    }
}
