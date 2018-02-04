using System;
using MunicipalityTaxes.Core;

namespace MunicipalityTaxes.Business.Taxes
{
    public interface ITaxService
    {
        decimal? Get(string municipality, DateTime date);

        ServiceResult CreateDailyTax(string municipality, DateTime day, decimal amount);

        // TODO: methods for other styles
    }
}
