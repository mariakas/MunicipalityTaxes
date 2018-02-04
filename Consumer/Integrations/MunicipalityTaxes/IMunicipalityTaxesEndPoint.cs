using System;
using System.Threading.Tasks;

namespace MunicipalityTaxes.Consumer.Integrations.MunicipalityTaxes
{
    public interface IMunicipalityTaxesEndPoint
    {
        decimal Get(string municipality, DateTime date);

        Task<decimal> GetAsync(string municipality, DateTime date);
    }
}
